using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;

namespace TSP
{
    public partial class MainForm : Form
    {
        private int citiesCount = 10;
        private int neurons = 20;
        private int iterations = 500;
        private double learningRate = 0.5;
        private double learningRadius = 0.5;

        private double[,] map = null;
        private Random rand = new Random();

        private Thread workerThread = null;
        private bool needToStop = false;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // initialize chart
            chart.AddDataSeries("cities", Color.Red, Chart.SeriesType.Dots, 5, false);
            chart.AddDataSeries("path", Color.Blue, Chart.SeriesType.Line, 1, false);
            chart.RangeX = new DoubleRange(0, 1000);
            chart.RangeY = new DoubleRange(0, 1000);

            //
            UpdateSettings();
            GenerateMap();
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
            citiesCountBox.Text = citiesCount.ToString();
            neuronsBox.Text = neurons.ToString();
            iterationsBox.Text = iterations.ToString();
            rateBox.Text = learningRate.ToString();
            radiusBox.Text = learningRadius.ToString();
        }

        // Generate new map for the Traivaling Salesman problem
        private void GenerateMap()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            // create coordinates array
            map = new double[citiesCount, 2];

            for (int i = 0; i < citiesCount; i++)
            {
                map[i, 0] = rand.Next(1001);
                map[i, 1] = rand.Next(1001);
            }

            // set the map
            chart.UpdateDataSeries("cities", map);
            // erase path if it is
            chart.UpdateDataSeries("path", null);
        }

        // On "Generate" button click - generate map
        private void generateMapButton_Click(object sender, System.EventArgs e)
        {
            // get cities count
            try
            {
                citiesCount = Math.Max(5, Math.Min(50, int.Parse(citiesCountBox.Text)));
            }
            catch
            {
                citiesCount = 20;
            }
            citiesCountBox.Text = citiesCount.ToString();

            // regenerate map
            GenerateMap();
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            this.BeginInvoke(new MethodInvoker(
                delegate()
                {
                    neuronsBox.Enabled = enable;
                    iterationsBox.Enabled = enable;
                    rateBox.Enabled = enable;
                    radiusBox.Enabled = enable;

                    startButton.Enabled = enable;
                    generateMapButton.Enabled = enable;
                    stopButton.Enabled = !enable;
                }
            ));
        }

        // On "Start" button click
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // get network size
            try
            {
                neurons = Math.Max(5, Math.Min(50, int.Parse(neuronsBox.Text)));
            }
            catch
            {
                neurons = 20;
            }
            // get iterations count
            try
            {
                iterations = Math.Max(10, Math.Min(1000000, int.Parse(iterationsBox.Text)));
            }
            catch
            {
                iterations = 500;
            }
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1.0, double.Parse(rateBox.Text)));
            }
            catch
            {
                learningRate = 0.5;
            }
            // get learning radius
            try
            {
                learningRadius = Math.Max(0.00001, Math.Min(1.0, double.Parse(radiusBox.Text)));
            }
            catch
            {
                learningRadius = 0.5;
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
            // set random generators range
            Neuron.RandRange = new DoubleRange(0, 1000);

            // create network
            DistanceNetwork network = new DistanceNetwork(2, neurons);

            // create learning algorithm
            ElasticNetworkLearning trainer = new ElasticNetworkLearning(network);

            double fixedLearningRate = learningRate / 20;
            double driftingLearningRate = fixedLearningRate * 19;

            // path
            double[,] path = new double[neurons + 1, 2];

            // input
            double[] input = new double[2];

            // iterations
            int i = 0;

            // loop
            while (!needToStop)
            {
                // update learning speed & radius
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = learningRadius * (iterations - i) / iterations;

                // set network input
                int currentCity = rand.Next(citiesCount);
                input[0] = map[currentCity, 0];
                input[1] = map[currentCity, 1];

                // run one training iteration
                trainer.Run(input);

                // show current path
                for (int j = 0; j < neurons; j++)
                {
                    path[j, 0] = network[0][j][0];
                    path[j, 1] = network[0][j][1];
                }
                path[neurons, 0] = network[0][0][0];
                path[neurons, 1] = network[0][0][1];

                chart.UpdateDataSeries("path", path);

                // increase current iteration
                i++;

                this.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        // set current iteration's info
                        currentIterationBox.Text = i.ToString();
                    }
                ));

                // stop ?
                if (i >= iterations)
                    break;
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}
