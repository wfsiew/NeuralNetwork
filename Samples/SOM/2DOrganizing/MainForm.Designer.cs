namespace SOMOrganizing
{
    partial class MainForm
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Panel pointsPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.CheckBox showConnectionsCheck;
        private System.Windows.Forms.CheckBox showInactiveCheck;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sizeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox radiusBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox rateBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
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
            this.generateButton = new System.Windows.Forms.Button();
            this.pointsPanel = new BufferedPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.showInactiveCheck = new System.Windows.Forms.CheckBox();
            this.showConnectionsCheck = new System.Windows.Forms.CheckBox();
            this.mapPanel = new BufferedPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.radiusBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rateBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sizeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.generateButton);
            this.groupBox1.Controls.Add(this.pointsPanel);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 295);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Points";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(10, 260);
            this.generateButton.Name = "generateButton";
            this.generateButton.TabIndex = 1;
            this.generateButton.Text = "&Generate";
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // pointsPanel
            // 
            this.pointsPanel.BackColor = System.Drawing.Color.White;
            this.pointsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pointsPanel.Location = new System.Drawing.Point(10, 20);
            this.pointsPanel.Name = "pointsPanel";
            this.pointsPanel.Size = new System.Drawing.Size(200, 200);
            this.pointsPanel.TabIndex = 0;
            this.pointsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.pointsPanel_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.showInactiveCheck);
            this.groupBox2.Controls.Add(this.showConnectionsCheck);
            this.groupBox2.Controls.Add(this.mapPanel);
            this.groupBox2.Location = new System.Drawing.Point(240, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 295);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            // 
            // showInactiveCheck
            // 
            this.showInactiveCheck.Checked = true;
            this.showInactiveCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showInactiveCheck.Location = new System.Drawing.Point(10, 265);
            this.showInactiveCheck.Name = "showInactiveCheck";
            this.showInactiveCheck.Size = new System.Drawing.Size(160, 16);
            this.showInactiveCheck.TabIndex = 2;
            this.showInactiveCheck.Text = "Show Inactive Neurons";
            this.showInactiveCheck.CheckedChanged += new System.EventHandler(this.showInactiveCheck_CheckedChanged);
            // 
            // showConnectionsCheck
            // 
            this.showConnectionsCheck.Checked = true;
            this.showConnectionsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showConnectionsCheck.Location = new System.Drawing.Point(10, 240);
            this.showConnectionsCheck.Name = "showConnectionsCheck";
            this.showConnectionsCheck.Size = new System.Drawing.Size(150, 16);
            this.showConnectionsCheck.TabIndex = 1;
            this.showConnectionsCheck.Text = "Show Connections";
            this.showConnectionsCheck.CheckedChanged += new System.EventHandler(this.showConnectionsCheck_CheckedChanged);
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.White;
            this.mapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapPanel.Location = new System.Drawing.Point(10, 20);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(200, 200);
            this.mapPanel.TabIndex = 0;
            this.mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mapPanel_Paint);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.stopButton);
            this.groupBox3.Controls.Add(this.startButton);
            this.groupBox3.Controls.Add(this.currentIterationBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.radiusBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.rateBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.iterationsBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.sizeBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(470, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(180, 295);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Neural Network";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(95, 260);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 16;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 260);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 15;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(110, 160);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(60, 20);
            this.currentIterationBox.TabIndex = 14;
            this.currentIterationBox.Text = "";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 16);
            this.label8.TabIndex = 13;
            this.label8.Text = "Curren iteration:";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(10, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 2);
            this.label7.TabIndex = 12;
            // 
            // radiusBox
            // 
            this.radiusBox.Location = new System.Drawing.Point(110, 120);
            this.radiusBox.Name = "radiusBox";
            this.radiusBox.Size = new System.Drawing.Size(60, 20);
            this.radiusBox.TabIndex = 11;
            this.radiusBox.Text = "";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Initial radius:";
            // 
            // rateBox
            // 
            this.rateBox.Location = new System.Drawing.Point(110, 95);
            this.rateBox.Name = "rateBox";
            this.rateBox.Size = new System.Drawing.Size(60, 20);
            this.rateBox.TabIndex = 9;
            this.rateBox.Text = "";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Initial learning rate:";
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(110, 70);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(60, 20);
            this.iterationsBox.TabIndex = 7;
            this.iterationsBox.Text = "";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Iteraions:";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(10, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 2);
            this.label3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "(neurons count = size * size)";
            // 
            // sizeBox
            // 
            this.sizeBox.Location = new System.Drawing.Point(110, 20);
            this.sizeBox.Name = "sizeBox";
            this.sizeBox.Size = new System.Drawing.Size(60, 20);
            this.sizeBox.TabIndex = 1;
            this.sizeBox.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Size:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(659, 315);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Kohonen SOM 2D Organizing";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}

