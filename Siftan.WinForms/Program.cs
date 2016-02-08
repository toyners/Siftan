
namespace Siftan.WinForms
{
  using System;
  using System.Windows.Forms;
  using CommandLine;

  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main(String[] args)
    {
      CommandLineOptions commandLineOptions = new CommandLineOptions();
      var result = Parser.Default.ParseArguments(args, commandLineOptions);

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var logManager = new LogManager(commandLineOptions.ApplicationLog);
      BaseController controller = CreateController(commandLineOptions.ControllerName, logManager);
      MainForm mainForm = controller.CreateMainForm();
      Application.Run(mainForm);
    }

    private static BaseController CreateController(String controllerClassName, ILogManager logManager)
    {
      if (controllerClassName == typeof(Controller).FullName)
      {
        return new Controller(logManager);
      }

      if (controllerClassName == typeof(TaskController).FullName)
      {
        return new TaskController(logManager);
      }

      throw new ArgumentException(String.Format("'{0}' not recognised as a controller name.", controllerClassName));
    }
  }
}
