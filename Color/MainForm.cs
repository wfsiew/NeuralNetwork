using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace Color
{
    public partial class MainForm : Form
    {
        private DistanceNetwork network;
        private Bitmap mapBitmap;
        private Random rand = new Random();

        private int iterations = 5000;
        private double learningRate = 0.1;
        private double radius = 15;

        private Thread workerThread = null;
        private bool needToStop = false;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Create network
            network = new DistanceNetwork(3, 100 * 100);

            // Create map bitmap
            mapBitmap = new Bitmap(200, 200, PixelFormat.Format24bppRgb);

            //
            RandomizeNetwork();
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
            iterationsBox.Text = iterations.ToString();
            rateBox.Text = learningRate.ToString();
            radiusBox.Text = radius.ToString();
        }

        // On "Rundomize" button clicked
        private void randomizeButton_Click(object sender, System.EventArgs e)
        {
            RandomizeNetwork();
        }

        // Radnomize weights of network
        private void RandomizeNetwork()
        {
            Neuron.RandRange = new DoubleRange(0, 255);

            // randomize net
            network.Randomize();

            // update map
            UpdateMap();
        }

        // Update map from network weights
        private void UpdateMap()
        {
            // lock
            Monitor.Enter(this);

            // lock bitmap
            BitmapData mapData = mapBitmap.LockBits(new Rectangle(0, 0, 200, 200),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = mapData.Stride;
            int offset = stride - 200 * 3;
            Layer layer = network[0];

            unsafe
            {
                byte* ptr = (byte*)mapData.Scan0;

                // for all rows
                for (int y = 0, i = 0; y < 100; y++)
                {
                    // for all pixels
                    for (int x = 0; x < 100; x++, i++, ptr += 6)
                    {
                        Neuron neuron = layer[i];

                        // red
                        ptr[2] = ptr[2 + 3] = ptr[2 + stride] = ptr[2 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuron[0]));
                        // green
                        ptr[1] = ptr[1 + 3] = ptr[1 + stride] = ptr[1 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuron[1]));
                        // blue
                        ptr[0] = ptr[0 + 3] = ptr[0 + stride] = ptr[0 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuron[2]));
                    }

                    ptr += offset;
                    ptr += stride;
                }
            }

            // unlock image
            mapBitmap.UnlockBits(mapData);

            // unlock
            Monitor.Exit(this);

            // invalidate maps panel
            mapPanel.Invalidate();
        }

        // Paint map
        private void mapPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // lock
            Monitor.Enter(this);

            // drat image
            g.DrawImage(mapBitmap, 0, 0, 200, 200);

            // unlock
            Monitor.Exit(this);
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            this.BeginInvoke(new MethodInvoker(
                delegate()
                {
                    iterationsBox.Enabled = enable;
                    rateBox.Enabled = enable;
                    radiusBox.Enabled = enable;

                    startButton.Enabled = enable;
                    randomizeButton.Enabled = enable;
                    stopButton.Enabled = !enable;
                }
            ));
        }

        // On "Start" button click
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // get iterations count
            try
            {
                iterations = Math.Max(10, Math.Min(1000000, int.Parse(iterationsBox.Text)));
            }
            catch
            {
                iterations = 5000;
            }
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1.0, double.Parse(rateBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
            }
            // get radius
            try
            {
                radius = Math.Max(5, Math.Min(75, int.Parse(radiusBox.Text)));
            }
            catch
            {
                radius = 15;
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
            // create learning algorithm
            SOMLearning trainer = new SOMLearning(network);

            // input
            double[] input = new double[3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;

            // loop
            while (!needToStop)
            {
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = (double)radius * (iterations - i) / iterations;

                input[0] = rand.Next(256);
                input[1] = rand.Next(256);
                input[2] = rand.Next(256);

                trainer.Run(input);

                // update map once per 50 iterations
                if ((i % 10) == 9)
                {
                    UpdateMap();
                }

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
