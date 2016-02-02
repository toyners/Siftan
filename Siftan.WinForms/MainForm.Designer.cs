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
      this.LineIDIndex_Spinner = new System.Windows.Forms.NumericUpDown();
      this.TermIndex_Spinner = new System.Windows.Forms.NumericUpDown();
      this.Delimiter_TextBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.Qualifier_TextBox = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.HeaderLineID_TextBox = new System.Windows.Forms.TextBox();
      this.TermLineID_TextBox = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.InputDirectory_TextBox = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.InputFileName_TextBox = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.Results_TextBox = new System.Windows.Forms.TextBox();
      this.Start_Button = new System.Windows.Forms.Button();
      this.label11 = new System.Windows.Forms.Label();
      this.MatchedOutputFileName_TextBox = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.Label();
      this.OutputDirectory_TextBox = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.UnmatchedOutputFileName_TextBox = new System.Windows.Forms.TextBox();
      this.InList_TextBox = new System.Windows.Forms.TextBox();
      this.SearchSubdirectories_CheckBox = new System.Windows.Forms.CheckBox();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.Cancel_Button = new System.Windows.Forms.Button();
      this.CreateUnmatchedOutput_CheckBox = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.LineIDIndex_Spinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TermIndex_Spinner)).BeginInit();
      this.SuspendLayout();
      // 
      // LineIDIndex_Spinner
      // 
      this.LineIDIndex_Spinner.Location = new System.Drawing.Point(165, 90);
      this.LineIDIndex_Spinner.Name = "LineIDIndex_Spinner";
      this.LineIDIndex_Spinner.Size = new System.Drawing.Size(42, 20);
      this.LineIDIndex_Spinner.TabIndex = 4;
      // 
      // TermIndex_Spinner
      // 
      this.TermIndex_Spinner.Location = new System.Drawing.Point(165, 142);
      this.TermIndex_Spinner.Name = "TermIndex_Spinner";
      this.TermIndex_Spinner.Size = new System.Drawing.Size(42, 20);
      this.TermIndex_Spinner.TabIndex = 6;
      this.TermIndex_Spinner.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
      // 
      // Delimiter_TextBox
      // 
      this.Delimiter_TextBox.Location = new System.Drawing.Point(165, 12);
      this.Delimiter_TextBox.Name = "Delimiter_TextBox";
      this.Delimiter_TextBox.Size = new System.Drawing.Size(42, 20);
      this.Delimiter_TextBox.TabIndex = 1;
      this.Delimiter_TextBox.Text = "|";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Delimiter:";
      // 
      // Qualifier_TextBox
      // 
      this.Qualifier_TextBox.Location = new System.Drawing.Point(165, 38);
      this.Qualifier_TextBox.Name = "Qualifier_TextBox";
      this.Qualifier_TextBox.Size = new System.Drawing.Size(42, 20);
      this.Qualifier_TextBox.TabIndex = 2;
      this.Qualifier_TextBox.Text = "\'";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(48, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Qualifier:";
      // 
      // HeaderLineID_TextBox
      // 
      this.HeaderLineID_TextBox.Location = new System.Drawing.Point(165, 64);
      this.HeaderLineID_TextBox.Name = "HeaderLineID_TextBox";
      this.HeaderLineID_TextBox.Size = new System.Drawing.Size(80, 20);
      this.HeaderLineID_TextBox.TabIndex = 3;
      this.HeaderLineID_TextBox.Text = "01";
      // 
      // TermLineID_TextBox
      // 
      this.TermLineID_TextBox.Location = new System.Drawing.Point(165, 116);
      this.TermLineID_TextBox.Name = "TermLineID_TextBox";
      this.TermLineID_TextBox.Size = new System.Drawing.Size(80, 20);
      this.TermLineID_TextBox.TabIndex = 5;
      this.TermLineID_TextBox.Text = "02";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 67);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(82, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Header Line ID:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(11, 92);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(97, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Header Line Index:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 144);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(63, 13);
      this.label5.TabIndex = 3;
      this.label5.Text = "Term Index:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 119);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(48, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "Term ID:";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(12, 184);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(79, 13);
      this.label7.TabIndex = 6;
      this.label7.Text = "Input Directory:";
      // 
      // InputDirectory_TextBox
      // 
      this.InputDirectory_TextBox.Location = new System.Drawing.Point(165, 181);
      this.InputDirectory_TextBox.Name = "InputDirectory_TextBox";
      this.InputDirectory_TextBox.Size = new System.Drawing.Size(269, 20);
      this.InputDirectory_TextBox.TabIndex = 7;
      this.InputDirectory_TextBox.Text = "C:\\Projects\\Siftan\\Temp\\Siftan.WinForms.AcceptanceTests\\";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(12, 210);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(79, 13);
      this.label8.TabIndex = 8;
      this.label8.Text = "Input Filename:";
      // 
      // InputFileName_TextBox
      // 
      this.InputFileName_TextBox.Location = new System.Drawing.Point(165, 207);
      this.InputFileName_TextBox.Name = "InputFileName_TextBox";
      this.InputFileName_TextBox.Size = new System.Drawing.Size(178, 20);
      this.InputFileName_TextBox.TabIndex = 8;
      this.InputFileName_TextBox.Text = "Input.csv";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(12, 359);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(42, 13);
      this.label9.TabIndex = 10;
      this.label9.Text = "Values:";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(349, 15);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(45, 13);
      this.label10.TabIndex = 12;
      this.label10.Text = "Results:";
      // 
      // Results_TextBox
      // 
      this.Results_TextBox.Location = new System.Drawing.Point(440, 12);
      this.Results_TextBox.Multiline = true;
      this.Results_TextBox.Name = "Results_TextBox";
      this.Results_TextBox.ReadOnly = true;
      this.Results_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.Results_TextBox.Size = new System.Drawing.Size(421, 363);
      this.Results_TextBox.TabIndex = 11;
      this.Results_TextBox.TabStop = false;
      this.Results_TextBox.WordWrap = false;
      // 
      // Start_Button
      // 
      this.Start_Button.Location = new System.Drawing.Point(765, 412);
      this.Start_Button.Name = "Start_Button";
      this.Start_Button.Size = new System.Drawing.Size(96, 32);
      this.Start_Button.TabIndex = 14;
      this.Start_Button.Text = "Start";
      this.Start_Button.UseVisualStyleBackColor = true;
      this.Start_Button.Click += new System.EventHandler(this.Start_Button_Click);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(12, 284);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(132, 13);
      this.label11.TabIndex = 17;
      this.label11.Text = "Matched Output Filename:";
      // 
      // MatchedOutputFileName_TextBox
      // 
      this.MatchedOutputFileName_TextBox.Location = new System.Drawing.Point(165, 281);
      this.MatchedOutputFileName_TextBox.Name = "MatchedOutputFileName_TextBox";
      this.MatchedOutputFileName_TextBox.Size = new System.Drawing.Size(178, 20);
      this.MatchedOutputFileName_TextBox.TabIndex = 11;
      this.MatchedOutputFileName_TextBox.Text = "matched.csv";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(12, 258);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(87, 13);
      this.label12.TabIndex = 15;
      this.label12.Text = "Output Directory:";
      // 
      // OutputDirectory_TextBox
      // 
      this.OutputDirectory_TextBox.Location = new System.Drawing.Point(165, 255);
      this.OutputDirectory_TextBox.Name = "OutputDirectory_TextBox";
      this.OutputDirectory_TextBox.Size = new System.Drawing.Size(269, 20);
      this.OutputDirectory_TextBox.TabIndex = 10;
      this.OutputDirectory_TextBox.Text = "C:\\Projects\\Siftan\\Temp\\Siftan.WinForms.AcceptanceTests\\";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(12, 333);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(145, 13);
      this.label13.TabIndex = 19;
      this.label13.Text = "Unmatched Output Filename:";
      // 
      // UnmatchedOutputFileName_TextBox
      // 
      this.UnmatchedOutputFileName_TextBox.Location = new System.Drawing.Point(165, 330);
      this.UnmatchedOutputFileName_TextBox.Name = "UnmatchedOutputFileName_TextBox";
      this.UnmatchedOutputFileName_TextBox.Size = new System.Drawing.Size(178, 20);
      this.UnmatchedOutputFileName_TextBox.TabIndex = 12;
      this.UnmatchedOutputFileName_TextBox.Text = "unmatched.csv";
      // 
      // InList_TextBox
      // 
      this.InList_TextBox.Location = new System.Drawing.Point(165, 356);
      this.InList_TextBox.Multiline = true;
      this.InList_TextBox.Name = "InList_TextBox";
      this.InList_TextBox.Size = new System.Drawing.Size(178, 73);
      this.InList_TextBox.TabIndex = 13;
      this.InList_TextBox.Text = "12345";
      // 
      // SearchSubdirectories_CheckBox
      // 
      this.SearchSubdirectories_CheckBox.AutoSize = true;
      this.SearchSubdirectories_CheckBox.Location = new System.Drawing.Point(165, 232);
      this.SearchSubdirectories_CheckBox.Name = "SearchSubdirectories_CheckBox";
      this.SearchSubdirectories_CheckBox.Size = new System.Drawing.Size(133, 17);
      this.SearchSubdirectories_CheckBox.TabIndex = 9;
      this.SearchSubdirectories_CheckBox.Text = "Search Sub-directories";
      this.SearchSubdirectories_CheckBox.UseVisualStyleBackColor = true;
      // 
      // progressBar1
      // 
      this.progressBar1.Location = new System.Drawing.Point(440, 381);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(421, 25);
      this.progressBar1.TabIndex = 20;
      // 
      // Cancel_Button
      // 
      this.Cancel_Button.Enabled = false;
      this.Cancel_Button.Location = new System.Drawing.Point(440, 412);
      this.Cancel_Button.Name = "Cancel_Button";
      this.Cancel_Button.Size = new System.Drawing.Size(96, 32);
      this.Cancel_Button.TabIndex = 14;
      this.Cancel_Button.Text = "Cancel";
      this.Cancel_Button.UseVisualStyleBackColor = true;
      this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
      // 
      // CreateUnmatchedOutput_CheckBox
      // 
      this.CreateUnmatchedOutput_CheckBox.AutoSize = true;
      this.CreateUnmatchedOutput_CheckBox.Location = new System.Drawing.Point(165, 307);
      this.CreateUnmatchedOutput_CheckBox.Name = "CreateUnmatchedOutput_CheckBox";
      this.CreateUnmatchedOutput_CheckBox.Size = new System.Drawing.Size(163, 17);
      this.CreateUnmatchedOutput_CheckBox.TabIndex = 9;
      this.CreateUnmatchedOutput_CheckBox.Text = "Write out unmatched records";
      this.CreateUnmatchedOutput_CheckBox.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(873, 452);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.CreateUnmatchedOutput_CheckBox);
      this.Controls.Add(this.SearchSubdirectories_CheckBox);
      this.Controls.Add(this.InList_TextBox);
      this.Controls.Add(this.label13);
      this.Controls.Add(this.UnmatchedOutputFileName_TextBox);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.MatchedOutputFileName_TextBox);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.OutputDirectory_TextBox);
      this.Controls.Add(this.Cancel_Button);
      this.Controls.Add(this.Start_Button);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.Results_TextBox);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.InputFileName_TextBox);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.InputDirectory_TextBox);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.TermLineID_TextBox);
      this.Controls.Add(this.Qualifier_TextBox);
      this.Controls.Add(this.HeaderLineID_TextBox);
      this.Controls.Add(this.Delimiter_TextBox);
      this.Controls.Add(this.TermIndex_Spinner);
      this.Controls.Add(this.LineIDIndex_Spinner);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.Text = "Siftan";
      ((System.ComponentModel.ISupportInitialize)(this.LineIDIndex_Spinner)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TermIndex_Spinner)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NumericUpDown LineIDIndex_Spinner;
    private System.Windows.Forms.NumericUpDown TermIndex_Spinner;
    private System.Windows.Forms.TextBox Delimiter_TextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox Qualifier_TextBox;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox HeaderLineID_TextBox;
    private System.Windows.Forms.TextBox TermLineID_TextBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox InputDirectory_TextBox;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox InputFileName_TextBox;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox Results_TextBox;
    private System.Windows.Forms.Button Start_Button;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox MatchedOutputFileName_TextBox;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox OutputDirectory_TextBox;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox UnmatchedOutputFileName_TextBox;
    private System.Windows.Forms.TextBox InList_TextBox;
    private System.Windows.Forms.CheckBox SearchSubdirectories_CheckBox;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Button Cancel_Button;
    private System.Windows.Forms.CheckBox CreateUnmatchedOutput_CheckBox;
  }
}

