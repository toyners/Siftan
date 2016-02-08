
namespace Siftan.WinForms
{
  using System;
  using System.IO;
  using System.Reflection;
  using System.Windows.Forms;
  using Jabberwocky.Toolkit.Path;

  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main(String[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var logManager = CreateLogManager(args);
      BaseController controller = CreateController(logManager);
      MainForm mainForm = controller.CreateMainForm();
      Application.Run(mainForm);
    }

    public static ILogManager CreateLogManager(String[] args)
    {
      if (args.Length == 1)
      {
        return new LogManager(args[0]);
      }

      return new LogManager(CreateDefaultApplicationLogFilePath());
    }

    public static BaseController CreateController(ILogManager logManager)
    {
      return new Controller(logManager);
    }

    public static String CreateDefaultApplicationLogFilePath()
    {
      String assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return PathOperations.CompleteDirectoryPath(assemblyDirectory) +
             DateTime.Today.ToString("dd-MM-yyyy") + ".log";
    }
  }
}
