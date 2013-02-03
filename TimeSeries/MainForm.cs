using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.IO;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;

namespace TimeSeries
{
    public partial class MainForm : Form
    {
        private double[] data = null;
        private double[,] dataToShow = null;

        private double learningRate = 0.1;
        private double momentum = 0.0;
        private double sigmoidAlphaValue = 2.0;
        private int windowSize = 5;
        private int predictionSize = 1;
        private int iterations = 1000;

        private Thread workerThread = null;
        private bool needToStop = false;

        private double[,] windowDelimiter = new double[2, 2] { { 0, 0 }, { 0, 0 } };
        private double[,] predictionDelimiter = new double[2, 2] { { 0, 0 }, { 0, 0 } };

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // initializa chart control
            chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);
            chart.AddDataSeries("window", Color.LightGray, Chart.SeriesType.Line, 1, false);
            chart.AddDataSeries("prediction", Color.Gray, Chart.SeriesType.Line, 1, false);

            // update controls
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

        // Update settings controls
        private void UpdateSettings()
        {
            learningRateBox.Text = learningRate.ToString();
            momentumBox.Text = momentum.ToString();
            alphaBox.Text = sigmoidAlphaValue.ToString();
            windowSizeBox.Text = windowSize.ToString();
            predictionSizeBox.Text = predictionSize.ToString();
            iterationsBox.Text = iterations.ToString();
        }

        // Load data
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // read maximum 50 points
                double[] tempData = new double[50];

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;
                    int i = 0;

                    // read the data
                    while ((i < 50) && ((str = reader.ReadLine()) != null))
                    {
                        // parse the value
                        tempData[i] = double.Parse(str);

                        i++;
                    }

                    // allocate and set data
                    data = new double[i];
                    dataToShow = new double[i, 2];
                    Array.Copy(tempData, 0, data, 0, i);
                    for (int j = 0; j < i; j++)
                    {
                        dataToShow[j, 0] = j;
                        dataToShow[j, 1] = data[j];
                    }
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
                chart.RangeX = new DoubleRange(0, data.Length - 1);
                chart.UpdateDataSeries("data", dataToShow);
                chart.UpdateDataSeries("solution", null);
                // set delimiters
                UpdateDelimiters();
                // enable "Start" button
                startButton.Enabled = true;
            }
        }

        // Update delimiters on the chart
        private void UpdateDelimiters()
        {
            // window delimiter
            windowDelimiter[0, 0] = windowDelimiter[1, 0] = windowSize;
            windowDelimiter[0, 1] = chart.RangeY.Min;
            windowDelimiter[1, 1] = chart.RangeY.Max;
            chart.UpdateDataSeries("window", windowDelimiter);
            // prediction delimiter
            predictionDelimiter[0, 0] = predictionDelimiter[1, 0] = data.Length - 1 - predictionSize;
            predictionDelimiter[0, 1] = chart.RangeY.Min;
            predictionDelimiter[1, 1] = chart.RangeY.Max;
            chart.UpdateDataSeries("prediction", predictionDelimiter);
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            dataList.Items.Clear();
            // add new records
            for (int i = 0, n = data.GetLength(0); i < n; i++)
            {
                dataList.Items.Add(data[i].ToString());
            }
        }

        // Enable/disable controls
        private void EnableControls(bool enable)
        {
            this.BeginInvoke(new MethodInvoker(
                delegate()
                {
                    loadDataButton.Enabled = enable;
                    learningRateBox.Enabled = enable;
                    momentumBox.Enabled = enable;
                    alphaBox.Enabled = enable;
                    windowSizeBox.Enabled = enable;
                    predictionSizeBox.Enabled = enable;
                    iterationsBox.Enabled = enable;

                    startButton.Enabled = enable;
                    stopButton.Enabled = !enable;
                }
            ));
        }

        // On window size changed
        private void windowSizeBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdateWindowSize();
        }

        // On prediction changed
        private void predictionSizeBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdatePredictionSize();
        }

        // Update window size
        private void UpdateWindowSize()
        {
            if (data != null)
            {
                // get new window size value
                try
                {
                    windowSize = Math.Max(1, Math.Min(15, int.Parse(windowSizeBox.Text)));
                }
                catch
                {
                    windowSize = 5;
                }
                // check if we have too few data
                if (windowSize >= data.Length)
                    windowSize = 1;
                // update delimiters
                UpdateDelimiters();
            }
        }

        // Update prediction size
        private void UpdatePredictionSize()
        {
            if (data != null)
            {
                // get new prediction size value
                try
                {
                    predictionSize = Math.Max(1, Math.Min(10, int.Parse(predictionSizeBox.Text)));
                }
                catch
                {
                    predictionSize = 1;
                }
                // check if we have too few data
                if (data.Length - predictionSize - 1 < windowSize)
                    predictionSize = 1;
                // update delimiters
                UpdateDelimiters();
            }
        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // clear previous solution
            for (int j = 0, n = data.Length; j < n; j++)
            {
                if (dataList.Items[j].SubItems.Count > 1)
                    dataList.Items[j].SubItems.RemoveAt(1);
            }

            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
            }
            // get momentum
            try
            {
                momentum = Math.Max(0, Math.Min(0.5, double.Parse(momentumBox.Text)));
            }
            catch
            {
                momentum = 0;
            }
            // get sigmoid's alpha value
            try
            {
                sigmoidAlphaValue = Math.Max(0.001, Math.Min(50, double.Parse(alphaBox.Text)));
            }
            catch
            {
                sigmoidAlphaValue = 2;
            }
            // iterations
            try
            {
                iterations = Math.Max(0, int.Parse(iterationsBox.Text));
            }
            catch
            {
                iterations = 1000;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();
        }

        // On button "Stop"
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
            // number of learning samples
            int samples = data.Length - predictionSize - windowSize;
            // data transformation factor
            double factor = 1.7 / chart.RangeY.Length;
            double yMin = chart.RangeY.Min;
            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[windowSize];
                output[i] = new double[1];

                // set input
                for (int j = 0; j < windowSize; j++)
                {
                    input[i][j] = (data[i + j] - yMin) * factor - 0.85;
                }
                // set output
                output[i][0] = (data[i + windowSize] - yMin) * factor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                windowSize, windowSize * 2, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations
            int iteration = 1;

            // solution array
            int solutionSize = data.Length - windowSize;
            double[,] solution = new double[solutionSize, 2];
            double[] networkInput = new double[windowSize];

            // calculate X values to be used with solution function
            for (int j = 0; j < solutionSize; j++)
            {
                solution[j, 0] = j + windowSize;
            }

            // loop
            while (!needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution and learning and prediction errors
                double learningError = 0.0;
                double predictionError = 0.0;
                // go through all the data
                for (int i = 0, n = data.Length - windowSize; i < n; i++)
                {
                    // put values from current window as network's input
                    for (int j = 0; j < windowSize; j++)
                    {
                        networkInput[j] = (data[i + j] - yMin) * factor - 0.85;
                    }

                    // evalue the function
                    solution[i, 1] = (network.Compute(networkInput)[0] + 0.85) / factor + yMin;

                    // calculate prediction error
                    if (i >= n - predictionSize)
                    {
                        predictionError += Math.Abs(solution[i, 1] - data[windowSize + i]);
                    }
                    else
                    {
                        learningError += Math.Abs(solution[i, 1] - data[windowSize + i]);
                    }
                }
                // update solution on the chart
                chart.UpdateDataSeries("solution", solution);

                this.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        // set current iteration's info
                        currentIterationBox.Text = iteration.ToString();
                        currentLearningErrorBox.Text = learningError.ToString("F3");
                        currentPredictionErrorBox.Text = predictionError.ToString("F3");
                    }
                ));

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((iterations != 0) && (iteration > iterations))
                    break;
            }

            // show new solution
            
            this.BeginInvoke(new MethodInvoker(
                delegate()
                {
                    for (int j = windowSize, k = 0, n = data.Length; j < n; j++, k++)
                    {
                        dataList.Items[j].SubItems.Add(solution[k, 1].ToString());
                    }
                }
            ));

            // enable settings controls
            EnableControls(true);
        }
    }
}
