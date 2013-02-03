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

namespace Approximation
{
    public partial class MainForm : Form
    {
        private double[,] data = null;

        private double learningRate = 0.1;
        private double momentum = 0.0;
        private double sigmoidAlphaValue = 2.0;
        private int neuronsInFirstLayer = 20;
        private int iterations = 1000;

        private Thread workerThread = null;
        private bool needToStop = false;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // init chart control
            chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);

            // init controls
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
            neuronsBox.Text = neuronsInFirstLayer.ToString();
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
                double[,] tempData = new double[50, 2];
                double minX = double.MaxValue;
                double maxX = double.MinValue;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;
                    int i = 0;

                    // read the data
                    while ((i < 50) && ((str = reader.ReadLine()) != null))
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');
                        // parse X
                        tempData[i, 0] = double.Parse(strs[0]);
                        tempData[i, 1] = double.Parse(strs[1]);

                        // search for min value
                        if (tempData[i, 0] < minX)
                            minX = tempData[i, 0];
                        // search for max value
                        if (tempData[i, 0] > maxX)
                            maxX = tempData[i, 0];

                        i++;
                    }

                    // allocate and set data
                    data = new double[i, 2];
                    Array.Copy(tempData, 0, data, 0, i * 2);
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
                chart.RangeX = new DoubleRange(minX, maxX);
                chart.UpdateDataSeries("data", data);
                chart.UpdateDataSeries("solution", null);
                // enable "Start" button
                startButton.Enabled = true;
            }
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            dataList.Items.Clear();
            // add new records
            for (int i = 0, n = data.GetLength(0); i < n; i++)
            {
                dataList.Items.Add(data[i, 0].ToString());
                dataList.Items[i].SubItems.Add(data[i, 1].ToString());
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            this.BeginInvoke(new MethodInvoker(
                delegate()
                {
                    loadDataButton.Enabled = enable;
                    learningRateBox.Enabled = enable;
                    momentumBox.Enabled = enable;
                    alphaBox.Enabled = enable;
                    neuronsBox.Enabled = enable;
                    iterationsBox.Enabled = enable;

                    startButton.Enabled = enable;
                    stopButton.Enabled = !enable;
                }
            ));
        }

        // On button "Start"
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
            // get neurons count in first layer
            try
            {
                neuronsInFirstLayer = Math.Max(5, Math.Min(50, int.Parse(neuronsBox.Text)));
            }
            catch
            {
                neuronsInFirstLayer = 20;
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
            int samples = data.GetLength(0);
            // data transformation factor
            double yFactor = 1.7 / chart.RangeY.Length;
            double yMin = chart.RangeY.Min;
            double xFactor = 2.0 / chart.RangeX.Length;
            double xMin = chart.RangeX.Min;

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[1];
                output[i] = new double[1];

                // set input
                input[i][0] = (data[i, 0] - xMin) * xFactor - 1.0;
                // set output
                output[i][0] = (data[i, 1] - yMin) * yFactor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(
                new BipolarSigmoidFunction(sigmoidAlphaValue),
                1, neuronsInFirstLayer, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations
            int iteration = 1;

            // solution array
            double[,] solution = new double[50, 2];
            double[] networkInput = new double[1];

            // calculate X values to be used with solution function
            for (int j = 0; j < 50; j++)
            {
                solution[j, 0] = chart.RangeX.Min + (double)j * chart.RangeX.Length / 49;
            }

            // loop
            while (!needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution
                for (int j = 0; j < 50; j++)
                {
                    networkInput[0] = (solution[j, 0] - xMin) * xFactor - 1.0;
                    solution[j, 1] = (network.Compute(networkInput)[0] + 0.85) / yFactor + yMin;
                }
                chart.UpdateDataSeries("solution", solution);
                // calculate error
                double learningError = 0.0;
                for (int j = 0, k = data.GetLength(0); j < k; j++)
                {
                    networkInput[0] = input[j][0];
                    learningError += Math.Abs(data[j, 1] - ((network.Compute(networkInput)[0] + 0.85) / yFactor + yMin));
                }

                this.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        // set current iteration's info
                        currentIterationBox.Text = iteration.ToString();
                        currentErrorBox.Text = learningError.ToString("F3");
                    }
                ));

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((iterations != 0) && (iteration > iterations))
                    break;
            }


            // enable settings controls
            EnableControls(true);
        }
    }
}
