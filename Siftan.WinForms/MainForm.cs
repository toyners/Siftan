
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

    public MainForm(Controller controller)
    {
      controller.VerifyThatObjectIsNotNull("Parameter 'controller' is null.");
      this.controller = controller;

      InitializeComponent();
    }

    public String MatchedOutputFilePath { get; internal set; }
    public String UnmatchedOutputFilePath { get; internal set; }
    public String[] ValueList { get; internal set; }

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

    private void Start_Button_Click(Object sender, EventArgs e)
    {
      this.controller.StartProcess(this);
    }
  }
}
