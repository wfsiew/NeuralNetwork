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

namespace XORProblem
{
    public partial class MainForm : Form
    {
        private double learningRate = 0.1;
        private double momentum = 0.0;
        private double sigmoidAlphaValue = 2.0;
        private double learningErrorLimit = 0.1;
        private int sigmoidType = 0;
        private bool saveStatisticsToFiles = false;

        private Thread workerThread = null;
        private bool needToStop = false;
        
        delegate void UpdateTextboxDelegate(TextBox txt, string s);

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // update controls
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

        // Update settings controls
        private void UpdateSettings()
        {
            learningRateBox.Text = learningRate.ToString();
            momentumBox.Text = momentum.ToString();
            alphaBox.Text = sigmoidAlphaValue.ToString();
            errorLimitBox.Text = learningErrorLimit.ToString();
            sigmoidTypeCombo.SelectedIndex = sigmoidType;

            saveFilesCheck.Checked = saveStatisticsToFiles;
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            learningRateBox.Enabled = enable;
            momentumBox.Enabled = enable;
            alphaBox.Enabled = enable;
            errorLimitBox.Enabled = enable;
            sigmoidTypeCombo.Enabled = enable;
            saveFilesCheck.Enabled = enable;

            startButton.Enabled = enable;
            stopButton.Enabled = !enable;
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
            // get sigmoid's type
            sigmoidType = sigmoidTypeCombo.SelectedIndex;

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
            // initialize input and output values
            double[][] input = null;
            double[][] output = null;

            if (sigmoidType == 0)
            {
                // unipolar data
                input = new double[4][] {
											new double[] {0, 0},
											new double[] {0, 1},
											new double[] {1, 0},
											new double[] {1, 1}
										};
                output = new double[4][] {
											 new double[] {0},
											 new double[] {1},
											 new double[] {1},
											 new double[] {0}
										 };
            }
            else
            {
                // biipolar data
                input = new double[4][] {
											new double[] {-1, -1},
											new double[] {-1,  1},
											new double[] { 1, -1},
											new double[] { 1,  1}
										};
                output = new double[4][] {
											 new double[] {-1},
											 new double[] { 1},
											 new double[] { 1},
											 new double[] {-1}
										 };
            }

            // create perceptron
            ActivationNetwork network = new ActivationNetwork(
                (sigmoidType == 0) ?
                    (IActivationFunction)new SigmoidFunction(sigmoidAlphaValue) :
                    (IActivationFunction)new BipolarSigmoidFunction(sigmoidAlphaValue),
                2, 2, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = learningRate;
            teacher.Momentum = momentum;

            // iterations
            int iteration = 1;

            // statistic files
            StreamWriter errorsFile = null;

            try
            {
                // check if we need to save statistics to files
                if (saveStatisticsToFiles)
                {
                    // open files
                    errorsFile = File.CreateText("errors.csv");
                }

                // erros list
                ArrayList errorsList = new ArrayList();

                // loop
                while (!needToStop)
                {
                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(input, output);
                    errorsList.Add(error);

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show current iteration & error
                    UpdateTextbox(currentIterationBox, iteration.ToString());
                    //currentIterationBox.Text = iteration.ToString();
                    UpdateTextbox(currentErrorBox, error.ToString());
                    //currentErrorBox.Text = error.ToString();
                    
                    iteration++;

                    // check if we need to stop
                    if (error <= learningErrorLimit)
                        break;
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
    }
}
