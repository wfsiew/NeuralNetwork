using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;

namespace Classifier
{
    public partial class MainForm : Form
    {
        private int samples = 0;
        private int variables = 0;
        private double[,] data = null;
        private int[] classes = null;

        private double learningRate = 0.1;
        private bool saveStatisticsToFiles = false;

        private Thread workerThread = null;
        private bool needToStop = false;
        
        delegate void UpdateTextboxDelegate(TextBox txt, string s);
        delegate void UpdateControlDelegate(Control c, bool e);
        delegate void ClearListviewDelegate(ListView o);
        delegate void UpdateListviewDelegate(ListView o, string w, int i, string s);

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // initialize charts
            chart.AddDataSeries("class1", Color.Red, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("class2", Color.Blue, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("classifier", Color.Gray, Chart.SeriesType.Line, 1, false);

            errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.ConnectedDots, 3, false);

            // update some controls
            saveFilesCheck.Checked = saveStatisticsToFiles;
            UpdateSettings();
        }

        // On main form closing
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if worker thread is running
            if ((workerThread != null) && (workerThread.IsAlive))
            {
                needToStop = true;
                workerThread.Join();
            }
        }

        // On "Load" button click - load data
        private void loadButton_Click(object sender, System.EventArgs e)
        {
            // data file format:
            // X1, X2, ... Xn, class (0|1)

            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                // temp buffers (for 50 samples only)
                double[,] tempData = null;
                int[] tempClasses = new int[50];

                // min and max X values
                double minX = double.MaxValue;
                double maxX = double.MinValue;

                // samples count
                samples = 0;

                try
                {
                    string str = null;

                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);

                    // read the data
                    while ((samples < 50) && ((str = reader.ReadLine()) != null))
                    {
                        // split the string
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');

                        // allocate data array
                        if (samples == 0)
                        {
                            variables = strs.Length - 1;
                            tempData = new double[50, variables];
                        }

                        // parse data
                        for (int j = 0; j < variables; j++)
                        {
                            tempData[samples, j] = double.Parse(strs[j]);
                        }
                        tempClasses[samples] = int.Parse(strs[variables]);

                        // search for min value
                        if (tempData[samples, 0] < minX)
                            minX = tempData[samples, 0];
                        // search for max value
                        if (tempData[samples, 0] > maxX)
                            maxX = tempData[samples, 0];

                        samples++;
                    }

                    // allocate and set data
                    data = new double[samples, variables];
                    Array.Copy(tempData, 0, data, 0, samples * variables);
                    classes = new int[samples];
                    Array.Copy(tempClasses, 0, classes, 0, samples);

                    // clear current result
                    ClearCurrentSolution();
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

                // update list and chart
                UpdateDataListView();

                // show chart or not
                bool showChart = (variables == 2);

                if (showChart)
                {
                    chart.RangeX = new DoubleRange(minX, maxX);
                    ShowTrainingData();
                }

                chart.Visible = showChart;
                noVisualizationLabel.Visible = !showChart;

                // enable start button
                startButton.Enabled = true;
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            learningRateBox.Text = learningRate.ToString();
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all curent data and columns
            dataList.Items.Clear();
            dataList.Columns.Clear();

            // add columns
            for (int i = 0, n = variables; i < n; i++)
            {
                dataList.Columns.Add(string.Format("X{0}", i + 1),
                    50, HorizontalAlignment.Left);
            }
            dataList.Columns.Add("Class", 50, HorizontalAlignment.Left);

            // add items
            for (int i = 0; i < samples; i++)
            {
                dataList.Items.Add(data[i, 0].ToString());

                for (int j = 1; j < variables; j++)
                {
                    dataList.Items[i].SubItems.Add(data[i, j].ToString());
                }
                dataList.Items[i].SubItems.Add(classes[i].ToString());
            }
        }

        // Show training data on chart
        private void ShowTrainingData()
        {
            int class1Size = 0;
            int class2Size = 0;

            // calculate number of samples in each class
            for (int i = 0, n = samples; i < n; i++)
            {
                if (classes[i] == 0)
                    class1Size++;
                else
                    class2Size++;
            }

            // allocate classes arrays
            double[,] class1 = new double[class1Size, 2];
            double[,] class2 = new double[class2Size, 2];

            // fill classes arrays
            for (int i = 0, c1 = 0, c2 = 0; i < samples; i++)
            {
                if (classes[i] == 0)
                {
                    // class 1
                    class1[c1, 0] = data[i, 0];
                    class1[c1, 1] = data[i, 1];
                    c1++;
                }
                else
                {
                    // class 2
                    class2[c2, 0] = data[i, 0];
                    class2[c2, 1] = data[i, 1];
                    c2++;
                }
            }

            // updata chart control
            chart.UpdateDataSeries("class1", class1);
            chart.UpdateDataSeries("class2", class2);
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            UpdateControl(learningRateBox, enable);
            UpdateControl(loadButton, enable);
            UpdateControl(startButton, enable);
            UpdateControl(saveFilesCheck, enable);
            UpdateControl(stopButton, !enable);
        }

        // Clear current solution
        private void ClearCurrentSolution()
        {
            chart.UpdateDataSeries("classifier", null);
            errorChart.UpdateDataSeries("error", null);
            weightsList.Items.Clear();
        }

        // On button "Start" - start learning procedure
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
            }
            saveStatisticsToFiles = saveFilesCheck.Checked;

            // update settings controls
            UpdateSettings();

            // disable all settings controls
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();
        }

        // On button "Stop" - stop learning procedure
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // stop worker thread
            needToStop = true;
            workerThread.Join();
            workerThread = null;
        }

        // Worker thread
        void SearchSolution()
        {
            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[variables];
                output[i] = new double[1];

                // copy input
                for (int j = 0; j < variables; j++)
                    input[i][j] = data[i, j];
                // copy output
                output[i][0] = classes[i];
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(new ThresholdFunction(), variables, 1);
            ActivationNeuron neuron = network[0][0];
            // create teacher
            PerceptronLearning teacher = new PerceptronLearning(network);
            // set learning rate
            teacher.LearningRate = learningRate;

            // iterations
            int iteration = 1;

            // statistic files
            StreamWriter errorsFile = null;
            StreamWriter weightsFile = null;

            try
            {
                // check if we need to save statistics to files
                if (saveStatisticsToFiles)
                {
                    // open files
                    errorsFile = File.CreateText("errors.csv");
                    weightsFile = File.CreateText("weights.csv");
                }

                // erros list
                ArrayList errorsList = new ArrayList();

                // loop
                while (!needToStop)
                {
                    // save current weights
                    if (weightsFile != null)
                    {
                        for (int i = 0; i < variables; i++)
                        {
                            weightsFile.Write(neuron[i] + ";");
                        }
                        weightsFile.WriteLine(neuron.Threshold);
                    }

                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output);
                    errorsList.Add(error);

                    // show current iteration
                    UpdateTextbox(iterationsBox, iteration.ToString());
                    //iterationsBox.Text = iteration.ToString();

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show classifier in the case of 2 dimensional data
                    if ((neuron.InputsCount == 2) && (neuron[1] != 0))
                    {
                        double k = -neuron[0] / neuron[1];
                        double b = -neuron.Threshold / neuron[1];

                        double[,] classifier = new double[2, 2] {
							{ chart.RangeX.Min, chart.RangeX.Min * k + b },
							{ chart.RangeX.Max, chart.RangeX.Max * k + b }
																};
                        // update chart
                        chart.UpdateDataSeries("classifier", classifier);
                    }

                    // stop if no error
                    if (error == 0)
                        break;

                    iteration++;
                }

                // show perceptron's weights
                ClearListview(weightsList);
                //weightsList.Items.Clear();
                for (int i = 0; i < variables; i++)
                {
                    UpdateListview(weightsList, string.Format("Weight {0}", i + 1), i, neuron[i].ToString("F6"));
                    //weightsList.Items.Add(string.Format("Weight {0}", i + 1));
                    //weightsList.Items[i].SubItems.Add(neuron[i].ToString("F6"));
                }
                UpdateListview(weightsList, "Threshold", variables, neuron.Threshold.ToString("F6"));
                //weightsList.Items.Add("Threshold");
                //weightsList.Items[variables].SubItems.Add(neuron.Threshold.ToString("F6"));

                // show error's dynamics
                double[,] errors = new double[errorsList.Count, 2];

                for (int i = 0, n = errorsList.Count; i < n; i++)
                {
                    errors[i, 0] = i;
                    errors[i, 1] = (double)errorsList[i];
                }

                errorChart.RangeX = new DoubleRange(0, errorsList.Count - 1);
                errorChart.RangeY = new DoubleRange(0, samples);
                errorChart.UpdateDataSeries("error", errors);
            }
            catch (IOException)
            {
                MessageBox.Show("Failed writing file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // close files
                if (errorsFile != null)
                    errorsFile.Close();
                if (weightsFile != null)
                    weightsFile.Close();
            }

            // enable settings controls
            EnableControls(true);
        }
        
        private void UpdateTextbox(TextBox txt, string s)
        {
            if (txt.InvokeRequired)
            {
                txt.Invoke(new UpdateTextboxDelegate(UpdateTextbox), new object[] { txt, s });
            }

            else
            {
                txt.Text = s;
            }
        }
        
        private void UpdateControl(Control c, bool e)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new UpdateControlDelegate(UpdateControl), new object[] { c, e });
            }
            
            else
            {
                c.Enabled = e;
            }
        }
        
        private void ClearListview(ListView o)
        {
            if (o.InvokeRequired)
            {
                o.Invoke(new ClearListviewDelegate(ClearListview), new object[] { o });
            }
            
            else
            {
                o.Items.Clear();
            }
        }
        
        private void UpdateListview(ListView o, string w, int i, string s)
        {
            if (o.InvokeRequired)
            {
                o.Invoke(new UpdateListviewDelegate(UpdateListview), new object[] { o, w, i, s });
            }
            
            else
            {
                o.Items.Add(w);
                o.Items[i].SubItems.Add(s);
            }
        }
    }
}
