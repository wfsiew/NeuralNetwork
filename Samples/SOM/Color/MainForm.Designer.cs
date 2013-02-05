namespace Color
{
    partial class MainForm
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox rateBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox radiusBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button randomizeButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox currentIterationBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.randomizeButton = new System.Windows.Forms.Button();
            this.mapPanel = new BufferedPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.radiusBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rateBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.randomizeButton,
																					this.mapPanel});
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 265);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map";
            // 
            // randomizeButton
            // 
            this.randomizeButton.Location = new System.Drawing.Point(10, 230);
            this.randomizeButton.Name = "randomizeButton";
            this.randomizeButton.TabIndex = 1;
            this.randomizeButton.Text = "&Randomize";
            this.randomizeButton.Click += new System.EventHandler(this.randomizeButton_Click);
            // 
            // mapPanel
            // 
            this.mapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapPanel.Location = new System.Drawing.Point(10, 20);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(202, 202);
            this.mapPanel.TabIndex = 0;
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.currentIterationBox,
																					this.label5,
																					this.stopButton,
																					this.startButton,
																					this.label4,
																					this.radiusBox,
																					this.label3,
																					this.rateBox,
																					this.label2,
																					this.iterationsBox,
																					this.label1});
            this.groupBox2.Location = new System.Drawing.Point(240, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 265);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Neural Network";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(110, 120);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(70, 20);
            this.currentIterationBox.TabIndex = 10;
            this.currentIterationBox.Text = "";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Curren iteration:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(105, 230);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(20, 230);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 7;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(10, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 2);
            this.label4.TabIndex = 6;
            // 
            // radiusBox
            // 
            this.radiusBox.Location = new System.Drawing.Point(110, 70);
            this.radiusBox.Name = "radiusBox";
            this.radiusBox.Size = new System.Drawing.Size(70, 20);
            this.radiusBox.TabIndex = 5;
            this.radiusBox.Text = "";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Initial radius:";
            // 
            // rateBox
            // 
            this.rateBox.Location = new System.Drawing.Point(110, 45);
            this.rateBox.Name = "rateBox";
            this.rateBox.Size = new System.Drawing.Size(70, 20);
            this.rateBox.TabIndex = 3;
            this.rateBox.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Initial learning rate:";
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(110, 20);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(70, 20);
            this.iterationsBox.TabIndex = 1;
            this.iterationsBox.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Iteraions:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(439, 285);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox2,
																		  this.groupBox1});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Color Clustering using Kohonen SOM";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}

