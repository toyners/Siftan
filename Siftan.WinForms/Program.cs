
namespace Siftan.WinForms
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Threading.Tasks;
  using System.Windows.Forms;
  using Jabberwocky.Toolkit.Path;

  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var logManager = new LogManager(CreateDefaultApplicationLogFilePath());
      Controller controller = new Controller(logManager);
      MainForm mainForm = controller.CreateMainForm();
      Application.Run(mainForm);
    }

    public static String CreateDefaultApplicationLogFilePath()
    {
      String assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return PathOperations.CompleteDirectoryPath(assemblyDirectory) +
             DateTime.Today.ToString("dd-MM-yyyy") + ".log";
    }
  }
}
