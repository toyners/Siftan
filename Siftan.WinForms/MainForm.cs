
namespace Siftan.WinForms
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Jabberwocky.Toolkit.File;
  using Jabberwocky.Toolkit.Object;

  public partial class MainForm : Form
  {
    private readonly Controller controller;

    private Int64 currentFileSize;

    public MainForm(Controller controller)
    {
      controller.VerifyThatObjectIsNotNull("Parameter 'controller' is null.");
      this.controller = controller;

      InitializeComponent();
    }

    internal String OutputDirectory { get { return this.OutputDirectory_TextBox.Text; } }

    internal String MatchedOutputFilePath
    {
      get { return Path.Combine(this.OutputDirectory_TextBox.Text, this.MatchedOutputFileName_TextBox.Text); }
    }

    internal String UnmatchedOutputFilePath
    {
      get { return Path.Combine(this.OutputDirectory_TextBox.Text, this.UnmatchedOutputFileName_TextBox.Text); }
    }

    internal String[] ValueList { get { return this.InList_TextBox.Text.Split(new[] { "\r\n" }, StringSplitOptions.None); } }

    internal String Delimiter { get { return this.Delimiter_TextBox.Text; } }

    internal String InputFilePattern
    {
      get { return Path.Combine(this.InputDirectory_TextBox.Text, this.InputFileName_TextBox.Text); }
    }

    internal FilePatternResolver.SearchDepths InputFileSearchDepth
    {
      get 
      {
        return this.SearchSubdirectories_CheckBox.Checked ?
          FilePatternResolver.SearchDepths.AllDirectories :
          FilePatternResolver.SearchDepths.InitialDirectoryOnly;
      }
    }

    internal Char Qualifier { get { return this.Qualifier_TextBox.Text[0]; } }

    internal String HeaderLineID { get { return this.HeaderLineID_TextBox.Text; } }

    internal UInt32 LineIDIndex { get { return (UInt32)this.LineIDIndex_Spinner.Value; } }

    internal UInt32 TermIndex { get { return (UInt32)this.TermIndex_Spinner.Value; } }

    internal String TermLineID { get { return this.TermLineID_TextBox.Text; } }

    internal void DisplayLogMessage(String message)
    {
      if (this.InvokeRequired)
      {
        Action action = () => DisplayLogMessage(message);
        this.Invoke(action);
        return;
      }

      this.Results_TextBox.Text += message + "\r\n";
    }

    internal void SetCurrentFileSize(Int64 size)
    {
      this.currentFileSize = size;
    }

    internal void SetCurrentFilePosition(Int64 position)
    {
      if (this.InvokeRequired)
      {
        Action action = () => SetCurrentFilePosition(position);
        this.Invoke(action);
        return;
      }

      if (position >= this.currentFileSize)
      {
        this.progressBar1.Value = 100;
      }
      else
      {
        this.progressBar1.Value = (Int32)((100 * position) / this.currentFileSize);
      }
    }

    internal void JobFinished()
    {
      this.progressBar1.Value = 0;
      this.Cancel_Button.Text = "Cancel";
      this.Start_Button.Enabled = true;
    }

    private void Start_Button_Click(Object sender, EventArgs e)
    {
      this.Cancel_Button.Enabled = true;
      this.Start_Button.Enabled = false;
      this.controller.StartProcess();
    }

    private void Cancel_Button_Click(Object sender, EventArgs e)
    {
      this.Cancel_Button.Enabled = false;
      this.Cancel_Button.Text = "Cancelling...";
      this.controller.CancelProcess();
    }
  }
}
