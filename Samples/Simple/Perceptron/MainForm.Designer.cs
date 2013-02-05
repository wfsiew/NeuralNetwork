namespace Classifier
{
    partial class MainForm
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView dataList;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private AForge.Controls.Chart chart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox learningRateBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label noVisualizationLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView weightsList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label5;
        private AForge.Controls.Chart errorChart;
        private System.Windows.Forms.CheckBox saveFilesCheck;
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
            this.chart = new AForge.Controls.Chart();
            this.loadButton = new System.Windows.Forms.Button();
            this.dataList = new System.Windows.Forms.ListView();
            this.noVisualizationLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.errorChart = new AForge.Controls.Chart();
            this.label5 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.weightsList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFilesCheck = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.chart,
																					this.loadButton,
																					this.dataList,
																					this.noVisualizationLabel});
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 420);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // chart
            // 
            this.chart.Location = new System.Drawing.Point(10, 215);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(170, 170);
            this.chart.TabIndex = 2;
            this.chart.Text = "chart1";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(10, 390);
            this.loadButton.Name = "loadButton";
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "&Load";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // dataList
            // 
            this.dataList.FullRowSelect = true;
            this.dataList.GridLines = true;
            this.dataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dataList.Location = new System.Drawing.Point(10, 20);
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(170, 190);
            this.dataList.TabIndex = 0;
            this.dataList.View = System.Windows.Forms.View.Details;
            // 
            // noVisualizationLabel
            // 
            this.noVisualizationLabel.Location = new System.Drawing.Point(10, 215);
            this.noVisualizationLabel.Name = "noVisualizationLabel";
            this.noVisualizationLabel.Size = new System.Drawing.Size(170, 170);
            this.noVisualizationLabel.TabIndex = 2;
            this.noVisualizationLabel.Text = "Visualization is not available.";
            this.noVisualizationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noVisualizationLabel.Visible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.saveFilesCheck,
																					this.errorChart,
																					this.label5,
																					this.stopButton,
																					this.iterationsBox,
																					this.label4,
																					this.weightsList,
																					this.label3,
																					this.label2,
																					this.startButton,
																					this.learningRateBox,
																					this.label1});
            this.groupBox2.Location = new System.Drawing.Point(210, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 420);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Training";
            // 
            // errorChart
            // 
            this.errorChart.Location = new System.Drawing.Point(10, 270);
            this.errorChart.Name = "errorChart";
            this.errorChart.Size = new System.Drawing.Size(220, 140);
            this.errorChart.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Error\'s dynamics:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(155, 49);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(90, 50);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.ReadOnly = true;
            this.iterationsBox.Size = new System.Drawing.Size(50, 20);
            this.iterationsBox.TabIndex = 7;
            this.iterationsBox.Text = "";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Iterations:";
            // 
            // weightsList
            // 
            this.weightsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader1,
																						  this.columnHeader2});
            this.weightsList.FullRowSelect = true;
            this.weightsList.GridLines = true;
            this.weightsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.weightsList.Location = new System.Drawing.Point(10, 130);
            this.weightsList.Name = "weightsList";
            this.weightsList.Size = new System.Drawing.Size(220, 110);
            this.weightsList.TabIndex = 5;
            this.weightsList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Weight";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 100;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Perceptron weights:";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(10, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 2);
            this.label2.TabIndex = 3;
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(155, 19);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 2;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(90, 20);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(50, 20);
            this.learningRateBox.TabIndex = 1;
            this.learningRateBox.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Learning rate:";
            // 
            // saveFilesCheck
            // 
            this.saveFilesCheck.Location = new System.Drawing.Point(10, 80);
            this.saveFilesCheck.Name = "saveFilesCheck";
            this.saveFilesCheck.Size = new System.Drawing.Size(182, 16);
            this.saveFilesCheck.TabIndex = 11;
            this.saveFilesCheck.Text = "Save weights and errors to files";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(459, 440);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox2,
																		  this.groupBox1});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Perceptron Classifier";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}

