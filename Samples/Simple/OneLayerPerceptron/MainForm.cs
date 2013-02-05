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
        private double[,] data = null;
        private int[] classes = null;
        private int classesCount = 0;
        private int[] samplesPerClass = null;

        private double learningRate = 0.1;
        private bool saveStatisticsToFiles = false;

        private Thread workerThread = null;
        private bool needToStop = false;

        // color for data series
        private static Color[] dataSereisColors = new Color[10] {
																	 Color.Red,		Color.Blue,
																	 Color.Green,	Color.DarkOrange,
																	 Color.Violet,	Color.Brown,
																	 Color.Black,	Color.Pink,
																	 Color.Olive,	Color.Navy };
                                   
        delegate void UpdateTextboxDelegate(TextBox txt, string s);
        delegate void UpdateControlDelegate(Control c, bool e);
        delegate void ClearListviewDelegate(ListView o);
        delegate void UpdateListviewDelegate(ListView o, string n, string w, string s);

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // update some controls
            saveFilesCheck.Checked = saveStatisticsToFiles;
            UpdateSettings();

            // initialize charts
            errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.ConnectedDots, 3);
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

        // Load input data
        private void loadButton_Click(object sender, System.EventArgs e)
        {
            // data file format:
            // X1, X2, class

            // load maximum 10 classes !

            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                // temp buffers (for 200 samples only)
                double[,] tempData = new double[200, 2];
                int[] tempClasses = new int[200];

                // min and max X values
                double minX = double.MaxValue;
                double maxX = double.MinValue;

                // samples count
                samples = 0;
                // classes count
                classesCount = 0;
                samplesPerClass = new int[10];

                try
                {
                    string str = null;

                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);

                    // read the data
                    while ((samples < 200) && ((str = reader.ReadLine()) != null))
                    {
                        // split the string
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');

                        // check tokens count
                        if (strs.Length != 3)
                            throw new ApplicationException("Invalid file format");

                        // parse tokens
                        tempData[samples, 0] = double.Parse(strs[0]);
                        tempData[samples, 1] = double.Parse(strs[1]);
                        tempClasses[samples] = int.Parse(strs[2]);

                        // skip classes over 10, except only first 10 classes
                        if (tempClasses[samples] >= 10)
                            continue;

                        // count the amount of different classes
                        if (tempClasses[samples] >= classesCount)
                            classesCount = tempClasses[samples] + 1;
                        // count samples per class
                        samplesPerClass[tempClasses[samples]]++;

                        // search for min value
                        if (tempData[samples, 0] < minX)
                            minX = tempData[samples, 0];
                        // search for max value
                        if (tempData[samples, 0] > maxX)
                            maxX = tempData[samples, 0];

                        samples++;
                    }

                    // allocate and set data
                    data = new double[samples, 2];
                    Array.Copy(tempData, 0, data, 0, samples * 2);
                    classes = new int[samples];
                    Array.Copy(tempClasses, 0, classes, 0, samples);

                    // clear current result
                    weightsList.Items.Clear();
                    errorChart.UpdateDataSeries("error", null);
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

                // update chart
                chart.RangeX = new DoubleRange(minX, maxX);
                ShowTrainingData();

                // enable start button
                startButton.Enabled = true;
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            learningRateBox.Text = learningRate.ToString();
        }

        // Show training data on chart
        private void ShowTrainingData()
        {
            double[][,] dataSeries = new double[classesCount][,];
            int[] indexes = new int[classesCount];

            // allocate data arrays
            for (int i = 0; i < classesCount; i++)
            {
                dataSeries[i] = new double[samplesPerClass[i], 2];
            }

            // fill data arrays
            for (int i = 0; i < samples; i++)
            {
                // get sample's class
                int dataClass = classes[i];
                // copy data into appropriate array
                dataSeries[dataClass][indexes[dataClass], 0] = data[i, 0];
                dataSeries[dataClass][indexes[dataClass], 1] = data[i, 1];
                indexes[dataClass]++;
            }

            // remove all previous data series from chart control
            chart.RemoveAllDataSeries();

            // add new data series
            for (int i = 0; i < classesCount; i++)
            {
                string className = string.Format("class" + i);

                // add data series
                chart.AddDataSeries(className, dataSereisColors[i], Chart.SeriesType.Dots, 5);
                chart.UpdateDataSeries(className, dataSeries[i]);
                // add classifier
                chart.AddDataSeries(string.Format("classifier" + i), Color.Gray, Chart.SeriesType.Line, 1, false);
            }
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

        // On "Start" button click
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

        // On "Stop" button click
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
                input[i] = new double[2];
                output[i] = new double[classesCount];

                // set input
                input[i][0] = data[i, 0];
                input[i][1] = data[i, 1];
                // set output
                output[i][classes[i]] = 1;
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(new ThresholdFunction(), 2, classesCount);
            ActivationLayer layer = network[0];
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
                        for (int i = 0; i < classesCount; i++)
                        {
                            weightsFile.Write("neuron" + i + ";");
                            weightsFile.Write(layer[i][0] + ";");
                            weightsFile.Write(layer[i][1] + ";");
                            weightsFile.WriteLine(layer[i].Threshold);
                        }
                    }

                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output);
                    errorsList.Add(error);

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show current iteration
                    UpdateTextbox(iterationsBox, iteration.ToString());
                    //iterationsBox.Text = iteration.ToString();

                    // stop if no error
                    if (error == 0)
                        break;

                    // show classifiers
                    for (int j = 0; j < classesCount; j++)
                    {
                        double k = -layer[j][0] / layer[j][1];
                        double b = -layer[j].Threshold / layer[j][1];

                        double[,] classifier = new double[2, 2] {
							{ chart.RangeX.Min, chart.RangeX.Min * k + b },
							{ chart.RangeX.Max, chart.RangeX.Max * k + b }
																};

                        // update chart
                        chart.UpdateDataSeries(string.Format("classifier" + j), classifier);
                    }

                    iteration++;
                }

                // show perceptron's weights
                ClearListview(weightsList);
                //weightsList.Items.Clear();
                for (int i = 0; i < classesCount; i++)
                {
                    string neuronName = string.Format("Neuron {0}", i + 1);

                    // weight 0
                    UpdateListview(weightsList, neuronName, "Weight 1", layer[i][0].ToString("F6"));
                    //ListViewItem item = weightsList.Items.Add(neuronName);
                    //item.SubItems.Add("Weight 1");
                    //item.SubItems.Add(layer[i][0].ToString("F6"));
                    // weight 1
                    UpdateListview(weightsList, neuronName, "Weight 2", layer[i][1].ToString("F6"));
                    //item = weightsList.Items.Add(neuronName);
                    //item.SubItems.Add("Weight 2");
                    //item.SubItems.Add(layer[i][1].ToString("F6"));
                    // threshold
                    UpdateListview(weightsList, neuronName, "Threshold", layer[i].Threshold.ToString("F6"));
                    //item = weightsList.Items.Add(neuronName);
                    //item.SubItems.Add("Threshold");
                    //item.SubItems.Add(layer[i].Threshold.ToString("F6"));
                }

                // show error's dynamics
                double[,] errors = new double[errorsList.Count, 2];

                for (int i = 0, n = errorsList.Count; i < n; i++)
                {
                    errors[i, 0] = i;
                    errors[i, 1] = (double)errorsList[i];
                }

                errorChart.RangeX = new DoubleRange(0, errorsList.Count - 1);
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

        private void MainForm_Load(object sender, System.EventArgs e)
        {

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
        
        private void UpdateListview(ListView o, string n, string w, string s)
        {
            ListViewItem item = null;
            
            if (o.InvokeRequired)
            {
                o.Invoke(new UpdateListviewDelegate(UpdateListview), new object[] { o, n, w, s });
            }
            
            else
            {
                item = weightsList.Items.Add(n);
                item.SubItems.Add(w);
                item.SubItems.Add(s);
            }
        }
    }
}
