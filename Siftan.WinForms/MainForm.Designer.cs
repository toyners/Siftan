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
      ((System.ComponentModel.ISupportInitialize)(this.LineIDIndex_Spinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TermIndex_Spinner)).BeginInit();
      this.SuspendLayout();
      // 
      // LineIDIndex_Spinner
      // 
      this.LineIDIndex_Spinner.Location = new System.Drawing.Point(47, 53);
      this.LineIDIndex_Spinner.Name = "LineIDIndex_Spinner";
      this.LineIDIndex_Spinner.Size = new System.Drawing.Size(120, 20);
      this.LineIDIndex_Spinner.TabIndex = 0;
      // 
      // TermIndex_Spinner
      // 
      this.TermIndex_Spinner.Location = new System.Drawing.Point(47, 79);
      this.TermIndex_Spinner.Name = "TermIndex_Spinner";
      this.TermIndex_Spinner.Size = new System.Drawing.Size(120, 20);
      this.TermIndex_Spinner.TabIndex = 0;
      // 
      // Delimiter_TextBox
      // 
      this.Delimiter_TextBox.Location = new System.Drawing.Point(47, 121);
      this.Delimiter_TextBox.Name = "Delimiter_TextBox";
      this.Delimiter_TextBox.Size = new System.Drawing.Size(100, 20);
      this.Delimiter_TextBox.TabIndex = 1;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 261);
      this.Controls.Add(this.Delimiter_TextBox);
      this.Controls.Add(this.TermIndex_Spinner);
      this.Controls.Add(this.LineIDIndex_Spinner);
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
  }
}

