
namespace Siftan.Console.AcceptanceTests
{
  using System;
  using System.Diagnostics;
  using TestStack.White;
  using TestSupport;
  public static class ConsoleRunner
  {
    public static void Run(String command, String commandArguments)
    {
      ProcessStartInfo processStartInfo = new ProcessStartInfo(command, commandArguments);
      Application application = Application.Launch(processStartInfo);

      MethodRunner.RunForDuration(() => { return application.Process.HasExited; });

      if (!application.Process.HasExited)
      {
        throw new TimeoutException("Console application has hung.");
      }

      if (application.Process.ExitCode != 0)
      {
        throw new Exception(String.Format("Console application has finished with exit code {0}.", application.Process.ExitCode));
      }
    }
  }
}
