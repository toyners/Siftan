
namespace Siftan.WinForms
{
  using System;
  using System.IO;
  using System.Reflection;
  using CommandLine;
  using Jabberwocky.Toolkit.Path;

  /// <summary>
  /// Option class for WinForms application. In-memory representation of the command line arguments.
  /// </summary>
  public class CommandLineOptions
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineOptions"/> class.
    /// </summary>
    public CommandLineOptions()
    {
      this.ApplicationLog = CreateDefaultApplicationLogFilePath();
      this.ControllerName = typeof(BackgroundController).FullName;
    }

    /// <summary>
    /// Gets or sets the full path to where the application log should be.
    /// </summary>
    [Option('a', "applicationlog", Required = false, HelpText = "Full path to the application log file. Defaults to the using the location of the program executable.")]
    public String ApplicationLog { get; set; }

    /// <summary>
    /// Gets or sets the full name of the controller class to use.
    /// </summary>
    [Option('c', "controller", Required = false, HelpText = "Full class name of controller to use. Defaults to using the 'Siftan.WinForms.Controller' class.")]
    public String ControllerName { get; set; }

    private static String CreateDefaultApplicationLogFilePath()
    {
      var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return PathOperations.CompleteDirectoryPath(assemblyDirectory) +
             DateTime.Today.ToString("dd-MM-yyyy") + ".log";
    }
  }
}
