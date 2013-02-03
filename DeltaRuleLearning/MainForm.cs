﻿using System;
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
        private int classesCount = 0;
        private int[] samplesPerClass = null;
        private int neuronsCount = 0;

        private double learningRate = 0.1;
        private double sigmoidAlphaValue = 2.0;
        private double learningErrorLimit = 0.1;
        private double iterationLimit = 1000;
        private bool useOneNeuronForTwoClasses = false;
        private bool useErrorLimit = true;
        private bool saveStatisticsToFiles = false;

        private Thread workerThread = null;
        private bool needToStop = false;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // update settings controls
            UpdateSettings();

            // initialize charts
            errorChart.AddDataSeries("error", Color.Red, Chart.SeriesType.Line, 1);
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
            // X1, X2, ..., Xn, class

            // load maximum 10 classes !

            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;

                // temp buffers (for 200 samples only)
                double[,] tempData = null;
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

                        // allocate data array
                        if (samples == 0)
                        {
                            variables = strs.Length - 1;
                            tempData = new double[200, variables];
                        }

                        // parse data
                        for (int j = 0; j < variables; j++)
                        {
                            tempData[samples, j] = double.Parse(strs[j]);
                        }
                        tempClasses[samples] = int.Parse(strs[variables]);

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
                    data = new double[samples, variables];
                    Array.Copy(tempData, 0, data, 0, samples * variables);
                    classes = new int[samples];
                    Array.Copy(tempClasses, 0, classes, 0, samples);
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

                classesBox.Text = classesCount.ToString();
                oneNeuronForTwoCheck.Enabled = (classesCount == 2);

                // set neurons count
                neuronsCount = ((classesCount == 2) && (useOneNeuronForTwoClasses)) ? 1 : classesCount;
                neuronsBox.Text = neuronsCount.ToString();

                ClearSolution();
                startButton.Enabled = true;
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            learningRateBox.Text = learningRate.ToString();
            alphaBox.Text = sigmoidAlphaValue.ToString();
            errorLimitBox.Text = learningErrorLimit.ToString();
            iterationsBox.Text = iterationLimit.ToString();

            oneNeuronForTwoCheck.Checked = useOneNeuronForTwoClasses;
            errorLimitCheck.Checked = useErrorLimit;
            saveFilesCheck.Checked = saveStatisticsToFiles;
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

        // Use or not one neuron to classify two classes
        private void oneNeuronForTwoCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            useOneNeuronForTwoClasses = oneNeuronForTwoCheck.Checked;
            // update neurons count box
            neuronsCount = ((classesCount == 2) && (useOneNeuronForTwoClasses)) ? 1 : classesCount;
            neuronsBox.Text = neuronsCount.ToString();
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            this.BeginInvoke(new MethodInvoker(
                delegate()
                {
                    learningRateBox.Enabled = enable;
                    alphaBox.Enabled = enable;
                    errorLimitBox.Enabled = enable;
                    iterationsBox.Enabled = enable;
                    oneNeuronForTwoCheck.Enabled = ((enable) && (classesCount == 2));
                    errorLimitCheck.Enabled = enable;
                    saveFilesCheck.Enabled = enable;

                    loadButton.Enabled = enable;
                    startButton.Enabled = enable;
                    stopButton.Enabled = !enable;
                }
            ));
        }

        // Clear current solution
        private void ClearSolution()
        {
            errorChart.UpdateDataSeries("error", null);
            weightsList.Items.Clear();
            currentIterationBox.Text = string.Empty;
            currentErrorBox.Text = string.Empty;
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
            // get sigmoid's alpha value
            try
            {
                sigmoidAlphaValue = Math.Max(0.01, Math.Min(100, double.Parse(alphaBox.Text)));
            }
            catch
            {
                sigmoidAlphaValue = 2;
            }
            // get learning error limit
            try
            {
                learningErrorLimit = Math.Max(0, double.Parse(errorLimitBox.Text));
            }
            catch
            {
                learningErrorLimit = 0.1;
            }
            // get iterations limit
            try
            {
                iterationLimit = Math.Max(0, int.Parse(iterationsBox.Text));
            }
            catch
            {
                iterationLimit = 1000;
            }

            useOneNeuronForTwoClasses = oneNeuronForTwoCheck.Checked;
            useErrorLimit = errorLimitCheck.Checked;
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
            bool reducedNetwork = ((classesCount == 2) && (useOneNeuronForTwoClasses));

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[variables];
                output[i] = new double[neuronsCount];

                // set input
                for (int j = 0; j < variables; j++)
                    input[i][j] = data[i, j];
                // set output
                if (reducedNetwork)
                {
                    output[i][0] = classes[i];
                }
                else
                {
                    output[i][classes[i]] = 1;
                }
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(sigmoidAlphaValue), variables, neuronsCount);
            ActivationLayer layer = network[0];
            // create teacher
            DeltaRuleLearning teacher = new DeltaRuleLearning(network);
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
                        for (int i = 0; i < neuronsCount; i++)
                        {
                            weightsFile.Write("neuron" + i + ";");
                            for (int j = 0; j < variables; j++)
                                weightsFile.Write(layer[i][j] + ";");
                            weightsFile.WriteLine(layer[i].Threshold);
                        }
                    }

                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output) / samples;
                    errorsList.Add(error);

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    this.BeginInvoke(new MethodInvoker(
                        delegate()
                        {
                            // show current iteration & error
                            currentIterationBox.Text = iteration.ToString();
                            currentErrorBox.Text = error.ToString();
                        }
                    ));

                    iteration++;

                    // check if we need to stop
                    if ((useErrorLimit) && (error <= learningErrorLimit))
                        break;
                    if ((!useErrorLimit) && (iterationLimit != 0) && (iteration > iterationLimit))
                        break;
                }

                this.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        // show perceptron's weights
                        weightsList.Items.Clear();
                        for (int i = 0; i < neuronsCount; i++)
                        {
                            string neuronName = string.Format("Neuron {0}", i + 1);
                            ListViewItem item = null;

                            // add all weights
                            for (int j = 0; j < variables; j++)
                            {
                                item = weightsList.Items.Add(neuronName);
                                item.SubItems.Add(string.Format("Weight {0}", j + 1));
                                item.SubItems.Add(layer[i][0].ToString("F6"));
                            }
                            // threshold
                            item = weightsList.Items.Add(neuronName);
                            item.SubItems.Add("Threshold");
                            item.SubItems.Add(layer[i].Threshold.ToString("F6"));
                        }
                    }
                ));

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
    }
}
