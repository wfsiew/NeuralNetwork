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

namespace SOMOrganizing
{
    public partial class MainForm : Form
    {
        private const int groupRadius = 20;
        private const int pointsCount = 100;
        private int[,] points = new int[pointsCount, 2];	// x, y
        private double[][] trainingSet = new double[pointsCount][];
        private int[, ,] map;

        private int networkSize = 15;
        private int iterations = 500;
        private double learningRate = 0.3;
        private int learningRadius = 3;

        private Random rand = new Random();
        private Thread workerThread = null;
        private bool needToStop = false;
        
        delegate void UpdateTextboxDelegate(TextBox txt, string s);
        delegate void UpdateControlDelegate(Control c, bool e);

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            GeneratePoints();
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
            sizeBox.Text = networkSize.ToString();
            iterationsBox.Text = iterations.ToString();
            rateBox.Text = learningRate.ToString();
            radiusBox.Text = learningRadius.ToString();
        }

        // On "Generate" button click
        private void generateButton_Click(object sender, System.EventArgs e)
        {
            GeneratePoints();
        }

        // Generate point
        private void GeneratePoints()
        {
            int width = pointsPanel.ClientRectangle.Width;
            int height = pointsPanel.ClientRectangle.Height;
            int diameter = groupRadius * 2;

            // generate groups of ten points
            for (int i = 0; i < pointsCount; )
            {
                int cx = rand.Next(width);
                int cy = rand.Next(height);

                // generate group
                for (int j = 0; (i < pointsCount) && (j < 10); )
                {
                    int x = cx + rand.Next(diameter) - groupRadius;
                    int y = cy + rand.Next(diameter) - groupRadius;

                    // check if wee are not out
                    if ((x < 0) || (y < 0) || (x >= width) || (y >= height))
                    {
                        continue;
                    }

                    // add point
                    points[i, 0] = x;
                    points[i, 1] = y;

                    j++;
                    i++;
                }
            }

            map = null;
            pointsPanel.Invalidate();
            mapPanel.Invalidate();
        }

        // Paint points
        private void pointsPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Brush brush = new SolidBrush(Color.Blue))
            {
                // draw all points
                for (int i = 0, n = points.GetLength(0); i < n; i++)
                {
                    g.FillEllipse(brush, points[i, 0] - 2, points[i, 1] - 2, 5, 5);
                }
            }
        }

        // Paint map
        private void mapPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (map != null)
            {
                //
                bool showConnections = showConnectionsCheck.Checked;
                bool showInactive = showInactiveCheck.Checked;

                // pens and brushes
                Brush brush = new SolidBrush(Color.Blue);
                Brush brushGray = new SolidBrush(Color.FromArgb(192, 192, 192));
                Pen pen = new Pen(Color.Blue, 1);
                Pen penGray = new Pen(Color.FromArgb(192, 192, 192), 1);

                // lock
                Monitor.Enter(this);

                if (showConnections)
                {
                    // draw connections
                    for (int i = 0, n = map.GetLength(0); i < n; i++)
                    {
                        for (int j = 0, k = map.GetLength(1); j < k; j++)
                        {
                            if ((!showInactive) && (map[i, j, 2] == 0))
                                continue;

                            // left
                            if ((i > 0) && ((showInactive) || (map[i - 1, j, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i - 1, j, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i - 1, j, 0], map[i - 1, j, 1]);
                            }

                            // right
                            if ((i < n - 1) && ((showInactive) || (map[i + 1, j, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i + 1, j, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i + 1, j, 0], map[i + 1, j, 1]);
                            }

                            // top
                            if ((j > 0) && ((showInactive) || (map[i, j - 1, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i, j - 1, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i, j - 1, 0], map[i, j - 1, 1]);
                            }

                            // bottom
                            if ((j < k - 1) && ((showInactive) || (map[i, j + 1, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i, j + 1, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i, j + 1, 0], map[i, j + 1, 1]);
                            }
                        }
                    }
                }

                // draw the map
                for (int i = 0, n = map.GetLength(0); i < n; i++)
                {
                    for (int j = 0, k = map.GetLength(1); j < k; j++)
                    {
                        if ((!showInactive) && (map[i, j, 2] == 0))
                            continue;

                        // draw the point
                        g.FillEllipse((map[i, j, 2] == 0) ? brushGray : brush, map[i, j, 0] - 2, map[i, j, 1] - 2, 5, 5);
                    }
                }

                // unlock
                Monitor.Exit(this);

                brush.Dispose();
                brushGray.Dispose();
                pen.Dispose();
                penGray.Dispose();
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            UpdateControl(sizeBox, enable);
            UpdateControl(iterationsBox, enable);
            UpdateControl(rateBox, enable);
            UpdateControl(radiusBox, enable);
            UpdateControl(startButton, enable);
            UpdateControl(generateButton, enable);
            UpdateControl(stopButton, !enable);
        }

        // Show/hide connections on map
        private void showConnectionsCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            mapPanel.Invalidate();
        }

        // Show/hide inactive neurons on map
        private void showInactiveCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            mapPanel.Invalidate();
        }

        // On "Start" button click
        private void startButton_Click(object sender, System.EventArgs e)
        {
            // get network size
            try
            {
                networkSize = Math.Max(5, Math.Min(50, int.Parse(sizeBox.Text)));
            }
            catch
            {
                networkSize = 15;
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
                learningRate = 0.3;
            }
            // get radius
            try
            {
                learningRadius = Math.Max(1, Math.Min(30, int.Parse(radiusBox.Text)));
            }
            catch
            {
                learningRadius = 3;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // generate training set
            for (int i = 0; i < pointsCount; i++)
            {
                // create new training sample
                trainingSet[i] = new double[2] { points[i, 0], points[i, 1] };
            }

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
            Neuron.RandRange = new DoubleRange(0, Math.Max(pointsPanel.ClientRectangle.Width, pointsPanel.ClientRectangle.Height));

            // create network
            DistanceNetwork network = new DistanceNetwork(2, networkSize * networkSize);

            // create learning algorithm
            SOMLearning trainer = new SOMLearning(network, networkSize, networkSize);

            // create map
            map = new int[networkSize, networkSize, 3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;

            // loop
            while (!needToStop)
            {
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = (double)learningRadius * (iterations - i) / iterations;

                // run training epoch
                trainer.RunEpoch(trainingSet);

                // update map
                UpdateMap(network);

                // increase current iteration
                i++;

                // set current iteration's info
                UpdateTextbox(currentIterationBox, i.ToString());
                //currentIterationBox.Text = i.ToString();

                // stop ?
                if (i >= iterations)
                    break;
            }

            // enable settings controls
            EnableControls(true);
        }

        // Update map
        private void UpdateMap(DistanceNetwork network)
        {
            // get first layer
            Layer layer = network[0];

            // lock
            Monitor.Enter(this);

            // run through all neurons
            for (int i = 0, n = layer.NeuronsCount; i < n; i++)
            {
                Neuron neuron = layer[i];

                int x = i % networkSize;
                int y = i / networkSize;

                map[y, x, 0] = (int)neuron[0];
                map[y, x, 1] = (int)neuron[1];
                map[y, x, 2] = 0;
            }

            // collect active neurons
            for (int i = 0; i < pointsCount; i++)
            {
                network.Compute(trainingSet[i]);
                int w = network.GetWinner();

                map[w / networkSize, w % networkSize, 2] = 1;
            }

            // unlock
            Monitor.Exit(this);

            //
            mapPanel.Invalidate();
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
    }
}
