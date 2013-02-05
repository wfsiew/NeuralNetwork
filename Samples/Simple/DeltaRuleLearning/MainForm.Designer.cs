namespace Classifier
{
    partial class MainForm
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListView dataList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox learningRateBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox alphaBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox errorLimitBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox neuronsBox;
        private System.Windows.Forms.CheckBox oneNeuronForTwoCheck;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox classesBox;
        private System.Windows.Forms.CheckBox errorLimitCheck;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox currentErrorBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListView weightsList;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.CheckBox saveFilesCheck;
        private AForge.Controls.Chart errorChart;
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
            this.classesBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dataList = new System.Windows.Forms.ListView();
            this.loadButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.currentErrorBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.errorLimitCheck = new System.Windows.Forms.CheckBox();
            this.oneNeuronForTwoCheck = new System.Windows.Forms.CheckBox();
            this.neuronsBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorLimitBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.saveFilesCheck = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.weightsList = new System.Windows.Forms.ListView();
            this.errorChart = new AForge.Controls.Chart();
            this.label12 = new System.Windows.Forms.Label();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.classesBox,
																					this.label10,
																					this.dataList,
																					this.loadButton});
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 330);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // classesBox
            // 
            this.classesBox.Location = new System.Drawing.Point(190, 297);
            this.classesBox.Name = "classesBox";
            this.classesBox.ReadOnly = true;
            this.classesBox.Size = new System.Drawing.Size(30, 20);
            this.classesBox.TabIndex = 3;
            this.classesBox.Text = "";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(140, 299);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "Classes:";
            // 
            // dataList
            // 
            this.dataList.FullRowSelect = true;
            this.dataList.GridLines = true;
            this.dataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dataList.Location = new System.Drawing.Point(10, 20);
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(210, 270);
            this.dataList.TabIndex = 0;
            this.dataList.View = System.Windows.Forms.View.Details;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(10, 297);
            this.loadButton.Name = "loadButton";
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "&Load";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.currentErrorBox,
																					this.label11,
																					this.label9,
																					this.currentIterationBox,
																					this.label8,
																					this.label7,
																					this.errorLimitCheck,
																					this.oneNeuronForTwoCheck,
																					this.neuronsBox,
																					this.label6,
																					this.label5,
																					this.iterationsBox,
																					this.label4,
																					this.errorLimitBox,
																					this.label3,
																					this.alphaBox,
																					this.label2,
																					this.label1,
																					this.learningRateBox,
																					this.stopButton,
																					this.startButton});
            this.groupBox2.Location = new System.Drawing.Point(250, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 330);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Training";
            // 
            // currentErrorBox
            // 
            this.currentErrorBox.Location = new System.Drawing.Point(125, 255);
            this.currentErrorBox.Name = "currentErrorBox";
            this.currentErrorBox.ReadOnly = true;
            this.currentErrorBox.Size = new System.Drawing.Size(50, 20);
            this.currentErrorBox.TabIndex = 20;
            this.currentErrorBox.Text = "";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(10, 257);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 14);
            this.label11.TabIndex = 19;
            this.label11.Text = "Current average error:";
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(10, 283);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 2);
            this.label9.TabIndex = 18;
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(125, 230);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(50, 20);
            this.currentIterationBox.TabIndex = 17;
            this.currentIterationBox.Text = "";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "Current iteration:";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(10, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(165, 2);
            this.label7.TabIndex = 15;
            // 
            // errorLimitCheck
            // 
            this.errorLimitCheck.Location = new System.Drawing.Point(10, 190);
            this.errorLimitCheck.Name = "errorLimitCheck";
            this.errorLimitCheck.Size = new System.Drawing.Size(157, 25);
            this.errorLimitCheck.TabIndex = 14;
            this.errorLimitCheck.Text = "Use error limit (checked) or iterations limit";
            // 
            // oneNeuronForTwoCheck
            // 
            this.oneNeuronForTwoCheck.Enabled = false;
            this.oneNeuronForTwoCheck.Location = new System.Drawing.Point(10, 165);
            this.oneNeuronForTwoCheck.Name = "oneNeuronForTwoCheck";
            this.oneNeuronForTwoCheck.Size = new System.Drawing.Size(168, 15);
            this.oneNeuronForTwoCheck.TabIndex = 13;
            this.oneNeuronForTwoCheck.Text = "Use 1 neuron for 2 classes";
            this.oneNeuronForTwoCheck.CheckedChanged += new System.EventHandler(this.oneNeuronForTwoCheck_CheckedChanged);
            // 
            // neuronsBox
            // 
            this.neuronsBox.Location = new System.Drawing.Point(125, 135);
            this.neuronsBox.Name = "neuronsBox";
            this.neuronsBox.ReadOnly = true;
            this.neuronsBox.Size = new System.Drawing.Size(50, 20);
            this.neuronsBox.TabIndex = 12;
            this.neuronsBox.Text = "";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "Neurons:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.label5.Location = new System.Drawing.Point(125, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "( 0 - inifinity )";
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(125, 95);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(50, 20);
            this.iterationsBox.TabIndex = 9;
            this.iterationsBox.Text = "";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Iterations limit:";
            // 
            // errorLimitBox
            // 
            this.errorLimitBox.Location = new System.Drawing.Point(125, 70);
            this.errorLimitBox.Name = "errorLimitBox";
            this.errorLimitBox.Size = new System.Drawing.Size(50, 20);
            this.errorLimitBox.TabIndex = 7;
            this.errorLimitBox.Text = "";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Learning error limit:";
            // 
            // alphaBox
            // 
            this.alphaBox.Location = new System.Drawing.Point(125, 45);
            this.alphaBox.Name = "alphaBox";
            this.alphaBox.Size = new System.Drawing.Size(50, 20);
            this.alphaBox.TabIndex = 5;
            this.alphaBox.Text = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sigmoid\'s alpha value:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Learning rate:";
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(125, 20);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(50, 20);
            this.learningRateBox.TabIndex = 3;
            this.learningRateBox.Text = "";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(100, 297);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(10, 297);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 5;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.saveFilesCheck,
																					this.label13,
																					this.weightsList,
																					this.errorChart,
																					this.label12});
            this.groupBox3.Location = new System.Drawing.Point(445, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 330);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Solution";
            // 
            // saveFilesCheck
            // 
            this.saveFilesCheck.Location = new System.Drawing.Point(10, 305);
            this.saveFilesCheck.Name = "saveFilesCheck";
            this.saveFilesCheck.Size = new System.Drawing.Size(195, 15);
            this.saveFilesCheck.TabIndex = 4;
            this.saveFilesCheck.Text = "Save weights and errors to files";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(10, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 12);
            this.label13.TabIndex = 3;
            this.label13.Text = "Error\'s dynamics:";
            // 
            // weightsList
            // 
            this.weightsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader1,
																						  this.columnHeader2,
																						  this.columnHeader3});
            this.weightsList.FullRowSelect = true;
            this.weightsList.GridLines = true;
            this.weightsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.weightsList.Location = new System.Drawing.Point(10, 35);
            this.weightsList.Name = "weightsList";
            this.weightsList.Size = new System.Drawing.Size(200, 130);
            this.weightsList.TabIndex = 2;
            this.weightsList.View = System.Windows.Forms.View.Details;
            // 
            // errorChart
            // 
            this.errorChart.Location = new System.Drawing.Point(10, 185);
            this.errorChart.Name = "errorChart";
            this.errorChart.Size = new System.Drawing.Size(200, 110);
            this.errorChart.TabIndex = 1;
            this.errorChart.Text = "chart1";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(10, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Network weights:";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Neuron";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Weight";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(674, 350);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBox3,
																		  this.groupBox2,
																		  this.groupBox1});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Classifier using Delta Rule Learning";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}

