namespace Siftan.WinForms
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      this.RecordFormat_GroupBox = new System.Windows.Forms.GroupBox();
      this.RecordDescriptors_TabControl = new System.Windows.Forms.TabControl();
      this.Delimited_Tab = new System.Windows.Forms.TabPage();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.TermLineID_TextBox = new System.Windows.Forms.TextBox();
      this.Qualifier_TextBox = new System.Windows.Forms.TextBox();
      this.HeaderLineID_TextBox = new System.Windows.Forms.TextBox();
      this.Delimiter_TextBox = new System.Windows.Forms.TextBox();
      this.TermIndex_Spinner = new System.Windows.Forms.NumericUpDown();
      this.LineIDIndex_Spinner = new System.Windows.Forms.NumericUpDown();
      this.FixedWidth_Tab = new System.Windows.Forms.TabPage();
      this.label15 = new System.Windows.Forms.Label();
      this.TermLength_TextBox = new System.Windows.Forms.TextBox();
      this.label16 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.TermStart_TextBox = new System.Windows.Forms.TextBox();
      this.TermLineID_FW_TextBox = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.Label();
      this.HeaderLineID_FW_TextBox = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.LineIDLength_TextBox = new System.Windows.Forms.TextBox();
      this.LineIDStart_TextBox = new System.Windows.Forms.TextBox();
      this.Input_GroupBox = new System.Windows.Forms.GroupBox();
      this.SearchSubdirectories_CheckBox = new System.Windows.Forms.CheckBox();
      this.label8 = new System.Windows.Forms.Label();
      this.InputFileName_TextBox = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.InputDirectory_TextBox = new System.Windows.Forms.TextBox();
      this.Output_GroupBox = new System.Windows.Forms.GroupBox();
      this.label13 = new System.Windows.Forms.Label();
      this.UnmatchedOutputFileName_TextBox = new System.Windows.Forms.TextBox();
      this.CreateUnmatchedOutput_CheckBox = new System.Windows.Forms.CheckBox();
      this.label11 = new System.Windows.Forms.Label();
      this.MatchedOutputFileName_TextBox = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.Label();
      this.OutputDirectory_TextBox = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.InList_TextBox = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.Cancel_Button = new System.Windows.Forms.Button();
      this.Start_Button = new System.Windows.Forms.Button();
      this.Results_TextBox = new System.Windows.Forms.TextBox();
      this.RecordFormat_GroupBox.SuspendLayout();
      this.RecordDescriptors_TabControl.SuspendLayout();
      this.Delimited_Tab.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.TermIndex_Spinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.LineIDIndex_Spinner)).BeginInit();
      this.FixedWidth_Tab.SuspendLayout();
      this.Input_GroupBox.SuspendLayout();
      this.Output_GroupBox.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // RecordFormat_GroupBox
      // 
      this.RecordFormat_GroupBox.Controls.Add(this.RecordDescriptors_TabControl);
      this.RecordFormat_GroupBox.Location = new System.Drawing.Point(11, 7);
      this.RecordFormat_GroupBox.Name = "RecordFormat_GroupBox";
      this.RecordFormat_GroupBox.Size = new System.Drawing.Size(463, 211);
      this.RecordFormat_GroupBox.TabIndex = 21;
      this.RecordFormat_GroupBox.TabStop = false;
      this.RecordFormat_GroupBox.Text = "Record Format";
      // 
      // RecordDescriptors_TabControl
      // 
      this.RecordDescriptors_TabControl.Controls.Add(this.Delimited_Tab);
      this.RecordDescriptors_TabControl.Controls.Add(this.FixedWidth_Tab);
      this.RecordDescriptors_TabControl.Location = new System.Drawing.Point(10, 18);
      this.RecordDescriptors_TabControl.Name = "RecordDescriptors_TabControl";
      this.RecordDescriptors_TabControl.SelectedIndex = 0;
      this.RecordDescriptors_TabControl.Size = new System.Drawing.Size(447, 187);
      this.RecordDescriptors_TabControl.TabIndex = 0;
      // 
      // Delimited_Tab
      // 
      this.Delimited_Tab.Controls.Add(this.label5);
      this.Delimited_Tab.Controls.Add(this.label6);
      this.Delimited_Tab.Controls.Add(this.label4);
      this.Delimited_Tab.Controls.Add(this.label2);
      this.Delimited_Tab.Controls.Add(this.label3);
      this.Delimited_Tab.Controls.Add(this.label1);
      this.Delimited_Tab.Controls.Add(this.TermLineID_TextBox);
      this.Delimited_Tab.Controls.Add(this.Qualifier_TextBox);
      this.Delimited_Tab.Controls.Add(this.HeaderLineID_TextBox);
      this.Delimited_Tab.Controls.Add(this.Delimiter_TextBox);
      this.Delimited_Tab.Controls.Add(this.TermIndex_Spinner);
      this.Delimited_Tab.Controls.Add(this.LineIDIndex_Spinner);
      this.Delimited_Tab.Location = new System.Drawing.Point(4, 22);
      this.Delimited_Tab.Name = "Delimited_Tab";
      this.Delimited_Tab.Padding = new System.Windows.Forms.Padding(3);
      this.Delimited_Tab.Size = new System.Drawing.Size(439, 161);
      this.Delimited_Tab.TabIndex = 0;
      this.Delimited_Tab.Text = "Delimited";
      this.Delimited_Tab.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 137);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(63, 13);
      this.label5.TabIndex = 25;
      this.label5.Text = "Term Index:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 112);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(48, 13);
      this.label6.TabIndex = 27;
      this.label6.Text = "Term ID:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(5, 85);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(97, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "Header Line Index:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 34);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(48, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "Qualifier:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 60);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(82, 13);
      this.label3.TabIndex = 22;
      this.label3.Text = "Header Line ID:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Delimiter:";
      // 
      // TermLineID_TextBox
      // 
      this.TermLineID_TextBox.Location = new System.Drawing.Point(159, 109);
      this.TermLineID_TextBox.Name = "TermLineID_TextBox";
      this.TermLineID_TextBox.Size = new System.Drawing.Size(80, 20);
      this.TermLineID_TextBox.TabIndex = 29;
      this.TermLineID_TextBox.Text = "02";
      // 
      // Qualifier_TextBox
      // 
      this.Qualifier_TextBox.Location = new System.Drawing.Point(159, 31);
      this.Qualifier_TextBox.Name = "Qualifier_TextBox";
      this.Qualifier_TextBox.Size = new System.Drawing.Size(42, 20);
      this.Qualifier_TextBox.TabIndex = 24;
      this.Qualifier_TextBox.Text = "\'";
      // 
      // HeaderLineID_TextBox
      // 
      this.HeaderLineID_TextBox.Location = new System.Drawing.Point(159, 57);
      this.HeaderLineID_TextBox.Name = "HeaderLineID_TextBox";
      this.HeaderLineID_TextBox.Size = new System.Drawing.Size(80, 20);
      this.HeaderLineID_TextBox.TabIndex = 26;
      this.HeaderLineID_TextBox.Text = "01";
      // 
      // Delimiter_TextBox
      // 
      this.Delimiter_TextBox.Location = new System.Drawing.Point(159, 5);
      this.Delimiter_TextBox.Name = "Delimiter_TextBox";
      this.Delimiter_TextBox.Size = new System.Drawing.Size(42, 20);
      this.Delimiter_TextBox.TabIndex = 19;
      this.Delimiter_TextBox.Text = "|";
      // 
      // TermIndex_Spinner
      // 
      this.TermIndex_Spinner.Location = new System.Drawing.Point(159, 135);
      this.TermIndex_Spinner.Name = "TermIndex_Spinner";
      this.TermIndex_Spinner.Size = new System.Drawing.Size(42, 20);
      this.TermIndex_Spinner.TabIndex = 30;
      this.TermIndex_Spinner.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
      // 
      // LineIDIndex_Spinner
      // 
      this.LineIDIndex_Spinner.Location = new System.Drawing.Point(159, 83);
      this.LineIDIndex_Spinner.Name = "LineIDIndex_Spinner";
      this.LineIDIndex_Spinner.Size = new System.Drawing.Size(42, 20);
      this.LineIDIndex_Spinner.TabIndex = 28;
      // 
      // FixedWidth_Tab
      // 
      this.FixedWidth_Tab.Controls.Add(this.label15);
      this.FixedWidth_Tab.Controls.Add(this.TermLength_TextBox);
      this.FixedWidth_Tab.Controls.Add(this.label16);
      this.FixedWidth_Tab.Controls.Add(this.label17);
      this.FixedWidth_Tab.Controls.Add(this.TermStart_TextBox);
      this.FixedWidth_Tab.Controls.Add(this.TermLineID_FW_TextBox);
      this.FixedWidth_Tab.Controls.Add(this.label14);
      this.FixedWidth_Tab.Controls.Add(this.HeaderLineID_FW_TextBox);
      this.FixedWidth_Tab.Controls.Add(this.label9);
      this.FixedWidth_Tab.Controls.Add(this.label10);
      this.FixedWidth_Tab.Controls.Add(this.LineIDLength_TextBox);
      this.FixedWidth_Tab.Controls.Add(this.LineIDStart_TextBox);
      this.FixedWidth_Tab.Location = new System.Drawing.Point(4, 22);
      this.FixedWidth_Tab.Name = "FixedWidth_Tab";
      this.FixedWidth_Tab.Size = new System.Drawing.Size(439, 161);
      this.FixedWidth_Tab.TabIndex = 1;
      this.FixedWidth_Tab.Text = "Fixed Width";
      this.FixedWidth_Tab.UseVisualStyleBackColor = true;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(6, 138);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(70, 13);
      this.label15.TabIndex = 35;
      this.label15.Text = "Term Length:";
      // 
      // TermLength_TextBox
      // 
      this.TermLength_TextBox.Location = new System.Drawing.Point(159, 135);
      this.TermLength_TextBox.Name = "TermLength_TextBox";
      this.TermLength_TextBox.Size = new System.Drawing.Size(42, 20);
      this.TermLength_TextBox.TabIndex = 36;
      this.TermLength_TextBox.Text = "5";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(6, 112);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(59, 13);
      this.label16.TabIndex = 32;
      this.label16.Text = "Term Start:";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(6, 86);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(71, 13);
      this.label17.TabIndex = 33;
      this.label17.Text = "Term Line ID:";
      // 
      // TermStart_TextBox
      // 
      this.TermStart_TextBox.Location = new System.Drawing.Point(159, 109);
      this.TermStart_TextBox.Name = "TermStart_TextBox";
      this.TermStart_TextBox.Size = new System.Drawing.Size(42, 20);
      this.TermStart_TextBox.TabIndex = 34;
      this.TermStart_TextBox.Text = "13";
      // 
      // TermLineID_FW_TextBox
      // 
      this.TermLineID_FW_TextBox.Location = new System.Drawing.Point(159, 83);
      this.TermLineID_FW_TextBox.Name = "TermLineID_FW_TextBox";
      this.TermLineID_FW_TextBox.Size = new System.Drawing.Size(80, 20);
      this.TermLineID_FW_TextBox.TabIndex = 31;
      this.TermLineID_FW_TextBox.Text = "02";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(6, 60);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(82, 13);
      this.label14.TabIndex = 29;
      this.label14.Text = "Header Line ID:";
      // 
      // HeaderLineID_FW_TextBox
      // 
      this.HeaderLineID_FW_TextBox.Location = new System.Drawing.Point(159, 57);
      this.HeaderLineID_FW_TextBox.Name = "HeaderLineID_FW_TextBox";
      this.HeaderLineID_FW_TextBox.Size = new System.Drawing.Size(80, 20);
      this.HeaderLineID_FW_TextBox.TabIndex = 30;
      this.HeaderLineID_FW_TextBox.Text = "01";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(6, 34);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(80, 13);
      this.label9.TabIndex = 26;
      this.label9.Text = "Line ID Length:";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 8);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(69, 13);
      this.label10.TabIndex = 27;
      this.label10.Text = "Line ID Start:";
      // 
      // LineIDLength_TextBox
      // 
      this.LineIDLength_TextBox.Location = new System.Drawing.Point(159, 31);
      this.LineIDLength_TextBox.Name = "LineIDLength_TextBox";
      this.LineIDLength_TextBox.Size = new System.Drawing.Size(42, 20);
      this.LineIDLength_TextBox.TabIndex = 28;
      this.LineIDLength_TextBox.Text = "2";
      // 
      // LineIDStart_TextBox
      // 
      this.LineIDStart_TextBox.Location = new System.Drawing.Point(159, 5);
      this.LineIDStart_TextBox.Name = "LineIDStart_TextBox";
      this.LineIDStart_TextBox.Size = new System.Drawing.Size(42, 20);
      this.LineIDStart_TextBox.TabIndex = 25;
      this.LineIDStart_TextBox.Text = "0";
      // 
      // Input_GroupBox
      // 
      this.Input_GroupBox.Controls.Add(this.SearchSubdirectories_CheckBox);
      this.Input_GroupBox.Controls.Add(this.label8);
      this.Input_GroupBox.Controls.Add(this.InputFileName_TextBox);
      this.Input_GroupBox.Controls.Add(this.label7);
      this.Input_GroupBox.Controls.Add(this.InputDirectory_TextBox);
      this.Input_GroupBox.Location = new System.Drawing.Point(11, 224);
      this.Input_GroupBox.Name = "Input_GroupBox";
      this.Input_GroupBox.Size = new System.Drawing.Size(463, 79);
      this.Input_GroupBox.TabIndex = 22;
      this.Input_GroupBox.TabStop = false;
      this.Input_GroupBox.Text = "Input";
      // 
      // SearchSubdirectories_CheckBox
      // 
      this.SearchSubdirectories_CheckBox.AutoSize = true;
      this.SearchSubdirectories_CheckBox.Location = new System.Drawing.Point(322, 47);
      this.SearchSubdirectories_CheckBox.Name = "SearchSubdirectories_CheckBox";
      this.SearchSubdirectories_CheckBox.Size = new System.Drawing.Size(133, 17);
      this.SearchSubdirectories_CheckBox.TabIndex = 14;
      this.SearchSubdirectories_CheckBox.Text = "Search Sub-directories";
      this.SearchSubdirectories_CheckBox.UseVisualStyleBackColor = true;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(6, 48);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(52, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "Filename:";
      // 
      // InputFileName_TextBox
      // 
      this.InputFileName_TextBox.Location = new System.Drawing.Point(94, 45);
      this.InputFileName_TextBox.Name = "InputFileName_TextBox";
      this.InputFileName_TextBox.Size = new System.Drawing.Size(220, 20);
      this.InputFileName_TextBox.TabIndex = 13;
      this.InputFileName_TextBox.Text = "Input.csv";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 22);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(52, 13);
      this.label7.TabIndex = 10;
      this.label7.Text = "Directory:";
      // 
      // InputDirectory_TextBox
      // 
      this.InputDirectory_TextBox.Location = new System.Drawing.Point(94, 19);
      this.InputDirectory_TextBox.Name = "InputDirectory_TextBox";
      this.InputDirectory_TextBox.Size = new System.Drawing.Size(361, 20);
      this.InputDirectory_TextBox.TabIndex = 11;
      this.InputDirectory_TextBox.Text = "C:\\Projects\\Siftan\\Temp\\Siftan.WinForms.AcceptanceTests\\";
      // 
      // Output_GroupBox
      // 
      this.Output_GroupBox.Controls.Add(this.label13);
      this.Output_GroupBox.Controls.Add(this.UnmatchedOutputFileName_TextBox);
      this.Output_GroupBox.Controls.Add(this.CreateUnmatchedOutput_CheckBox);
      this.Output_GroupBox.Controls.Add(this.label11);
      this.Output_GroupBox.Controls.Add(this.MatchedOutputFileName_TextBox);
      this.Output_GroupBox.Controls.Add(this.label12);
      this.Output_GroupBox.Controls.Add(this.OutputDirectory_TextBox);
      this.Output_GroupBox.Location = new System.Drawing.Point(11, 310);
      this.Output_GroupBox.Name = "Output_GroupBox";
      this.Output_GroupBox.Size = new System.Drawing.Size(463, 124);
      this.Output_GroupBox.TabIndex = 23;
      this.Output_GroupBox.TabStop = false;
      this.Output_GroupBox.Text = "Output";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(7, 94);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(110, 13);
      this.label13.TabIndex = 24;
      this.label13.Text = "Unmatched Filename:";
      // 
      // UnmatchedOutputFileName_TextBox
      // 
      this.UnmatchedOutputFileName_TextBox.Location = new System.Drawing.Point(123, 91);
      this.UnmatchedOutputFileName_TextBox.Name = "UnmatchedOutputFileName_TextBox";
      this.UnmatchedOutputFileName_TextBox.Size = new System.Drawing.Size(178, 20);
      this.UnmatchedOutputFileName_TextBox.TabIndex = 23;
      this.UnmatchedOutputFileName_TextBox.Text = "unmatched.csv";
      // 
      // CreateUnmatchedOutput_CheckBox
      // 
      this.CreateUnmatchedOutput_CheckBox.AutoSize = true;
      this.CreateUnmatchedOutput_CheckBox.Location = new System.Drawing.Point(123, 71);
      this.CreateUnmatchedOutput_CheckBox.Name = "CreateUnmatchedOutput_CheckBox";
      this.CreateUnmatchedOutput_CheckBox.Size = new System.Drawing.Size(163, 17);
      this.CreateUnmatchedOutput_CheckBox.TabIndex = 18;
      this.CreateUnmatchedOutput_CheckBox.Text = "Write out unmatched records";
      this.CreateUnmatchedOutput_CheckBox.UseVisualStyleBackColor = true;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(6, 48);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(97, 13);
      this.label11.TabIndex = 22;
      this.label11.Text = "Matched Filename:";
      // 
      // MatchedOutputFileName_TextBox
      // 
      this.MatchedOutputFileName_TextBox.Location = new System.Drawing.Point(123, 45);
      this.MatchedOutputFileName_TextBox.Name = "MatchedOutputFileName_TextBox";
      this.MatchedOutputFileName_TextBox.Size = new System.Drawing.Size(178, 20);
      this.MatchedOutputFileName_TextBox.TabIndex = 20;
      this.MatchedOutputFileName_TextBox.Text = "matched.csv";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(6, 22);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(52, 13);
      this.label12.TabIndex = 21;
      this.label12.Text = "Directory:";
      // 
      // OutputDirectory_TextBox
      // 
      this.OutputDirectory_TextBox.Location = new System.Drawing.Point(123, 19);
      this.OutputDirectory_TextBox.Name = "OutputDirectory_TextBox";
      this.OutputDirectory_TextBox.Size = new System.Drawing.Size(332, 20);
      this.OutputDirectory_TextBox.TabIndex = 19;
      this.OutputDirectory_TextBox.Text = "C:\\Projects\\Siftan\\Temp\\Siftan.WinForms.AcceptanceTests\\";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.InList_TextBox);
      this.groupBox1.Location = new System.Drawing.Point(11, 440);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(463, 92);
      this.groupBox1.TabIndex = 24;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Values";
      // 
      // InList_TextBox
      // 
      this.InList_TextBox.Location = new System.Drawing.Point(9, 19);
      this.InList_TextBox.Multiline = true;
      this.InList_TextBox.Name = "InList_TextBox";
      this.InList_TextBox.Size = new System.Drawing.Size(444, 61);
      this.InList_TextBox.TabIndex = 14;
      this.InList_TextBox.Text = "12345";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.progressBar1);
      this.groupBox2.Controls.Add(this.Cancel_Button);
      this.groupBox2.Controls.Add(this.Start_Button);
      this.groupBox2.Controls.Add(this.Results_TextBox);
      this.groupBox2.Location = new System.Drawing.Point(480, 7);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(405, 525);
      this.groupBox2.TabIndex = 25;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Results";
      // 
      // progressBar1
      // 
      this.progressBar1.Location = new System.Drawing.Point(6, 450);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(393, 25);
      this.progressBar1.TabIndex = 24;
      // 
      // Cancel_Button
      // 
      this.Cancel_Button.Enabled = false;
      this.Cancel_Button.Location = new System.Drawing.Point(6, 481);
      this.Cancel_Button.Name = "Cancel_Button";
      this.Cancel_Button.Size = new System.Drawing.Size(96, 32);
      this.Cancel_Button.TabIndex = 22;
      this.Cancel_Button.Text = "Cancel";
      this.Cancel_Button.UseVisualStyleBackColor = true;
      // 
      // Start_Button
      // 
      this.Start_Button.Location = new System.Drawing.Point(303, 481);
      this.Start_Button.Name = "Start_Button";
      this.Start_Button.Size = new System.Drawing.Size(96, 32);
      this.Start_Button.TabIndex = 23;
      this.Start_Button.Text = "Start";
      this.Start_Button.UseVisualStyleBackColor = true;
      this.Start_Button.Click += new System.EventHandler(this.Start_Button_Click);
      // 
      // Results_TextBox
      // 
      this.Results_TextBox.Location = new System.Drawing.Point(6, 19);
      this.Results_TextBox.Multiline = true;
      this.Results_TextBox.Name = "Results_TextBox";
      this.Results_TextBox.ReadOnly = true;
      this.Results_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.Results_TextBox.Size = new System.Drawing.Size(393, 425);
      this.Results_TextBox.TabIndex = 21;
      this.Results_TextBox.TabStop = false;
      this.Results_TextBox.WordWrap = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(895, 544);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.Output_GroupBox);
      this.Controls.Add(this.Input_GroupBox);
      this.Controls.Add(this.RecordFormat_GroupBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.Text = "Siftan";
      this.RecordFormat_GroupBox.ResumeLayout(false);
      this.RecordDescriptors_TabControl.ResumeLayout(false);
      this.Delimited_Tab.ResumeLayout(false);
      this.Delimited_Tab.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.TermIndex_Spinner)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.LineIDIndex_Spinner)).EndInit();
      this.FixedWidth_Tab.ResumeLayout(false);
      this.FixedWidth_Tab.PerformLayout();
      this.Input_GroupBox.ResumeLayout(false);
      this.Input_GroupBox.PerformLayout();
      this.Output_GroupBox.ResumeLayout(false);
      this.Output_GroupBox.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.GroupBox RecordFormat_GroupBox;
    private System.Windows.Forms.TabControl RecordDescriptors_TabControl;
    private System.Windows.Forms.TabPage Delimited_Tab;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TermLineID_TextBox;
    private System.Windows.Forms.TextBox Qualifier_TextBox;
    private System.Windows.Forms.TextBox HeaderLineID_TextBox;
    private System.Windows.Forms.TextBox Delimiter_TextBox;
    private System.Windows.Forms.NumericUpDown TermIndex_Spinner;
    private System.Windows.Forms.NumericUpDown LineIDIndex_Spinner;
    private System.Windows.Forms.GroupBox Input_GroupBox;
    private System.Windows.Forms.CheckBox SearchSubdirectories_CheckBox;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox InputFileName_TextBox;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox InputDirectory_TextBox;
    private System.Windows.Forms.GroupBox Output_GroupBox;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox UnmatchedOutputFileName_TextBox;
    private System.Windows.Forms.CheckBox CreateUnmatchedOutput_CheckBox;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox MatchedOutputFileName_TextBox;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox OutputDirectory_TextBox;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox InList_TextBox;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Button Cancel_Button;
    private System.Windows.Forms.Button Start_Button;
    private System.Windows.Forms.TextBox Results_TextBox;
    private System.Windows.Forms.TabPage FixedWidth_Tab;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox LineIDLength_TextBox;
    private System.Windows.Forms.TextBox LineIDStart_TextBox;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TextBox TermLength_TextBox;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.TextBox TermStart_TextBox;
    private System.Windows.Forms.TextBox TermLineID_FW_TextBox;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.TextBox HeaderLineID_FW_TextBox;
  }
}

