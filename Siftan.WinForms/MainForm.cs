
namespace Siftan.WinForms
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Jabberwocky.Toolkit.File;
  using Jabberwocky.Toolkit.Object;

  public partial class MainForm : Form, IDelimitedRecordDescriptorSource, IFixedWidthRecordDescriptorSource
  {
    #region Fields
    private readonly Controller controller;

    private Int64 currentFileSize;
    #endregion

    #region Construction
    public MainForm(Controller controller)
    {
      controller.VerifyThatObjectIsNotNull("Parameter 'controller' is null.");
      this.controller = controller;

      InitializeComponent();
    }
    #endregion

    #region Properties
    public Boolean HasDelimitedRecord { get { return this.RecordDescriptors_TabControl.SelectedTab == this.Delimited_Tab; } }

    public Boolean HasFixedWidthRecord { get { return this.RecordDescriptors_TabControl.SelectedTab == this.FixedWidth_Tab; } }

    internal String OutputDirectory { get { return this.OutputDirectory_TextBox.Text; } }

    internal String MatchedOutputFilePath
    {
      get { return Path.Combine(this.OutputDirectory_TextBox.Text, this.MatchedOutputFileName_TextBox.Text); }
    }

    internal String UnmatchedOutputFilePath
    {
      get
      {
        if (this.CreateUnmatchedOutput_CheckBox.Checked)
        {
          return Path.Combine(this.OutputDirectory_TextBox.Text, this.UnmatchedOutputFileName_TextBox.Text);
        }

        return null;
      }
    }

    internal String[] ValueList { get { return this.InList_TextBox.Text.Split(new[] { "\r\n" }, StringSplitOptions.None); } }

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
    #endregion

    #region Methods
    public DelimitedRecordDescriptor GetDelimitedRecord()
    {
      return new DelimitedRecordDescriptor
      {
        Delimiter = this.Delimiter_TextBox.Text,
        Qualifier = this.Qualifier_TextBox.Text[0],
        HeaderID = this.HeaderLineID_TextBox.Text,
        LineIDIndex = (UInt32)this.LineIDIndex_Spinner.Value,
        Term = new DelimitedRecordDescriptor.TermDefinition(
          this.TermLineID_TextBox.Text,
          (UInt32)this.TermIndex_Spinner.Value)
      };
    }

    public FixedWidthRecordDescriptor GetFixedWidthRecord()
    {
      return new FixedWidthRecordDescriptor
      {
        LineIDStart = UInt32.Parse(this.LineIDStart_TextBox.Text),
        LineIDLength = UInt32.Parse(this.LineIDLength_TextBox.Text),
        HeaderID = this.HeaderLineID_FW_TextBox.Text,
        Term = new FixedWidthRecordDescriptor.TermDefinition(
          this.TermLineID_FW_TextBox.Text,
          UInt32.Parse(this.TermStart_TextBox.Text),
          UInt32.Parse(this.TermLength_TextBox.Text))
      };
    }

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
    #endregion
  }
}
