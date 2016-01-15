
namespace Siftan.AcceptanceTests
{
  using System;
  using System.Diagnostics;
  using System.Threading;
  using TestStack.White;

  public static class ConsoleRunner
  {
    public static void Run(String command, String commandArguments, UInt32 retryCount = 5, UInt32 sleepDuration = 1000)
    {
      Int32 duration = (Int32)sleepDuration;

      ProcessStartInfo processStartInfo = new ProcessStartInfo(command, commandArguments);

      Application application = Application.Launch(processStartInfo);
      Boolean hasExited = false;
      while ((hasExited = application.Process.HasExited) == false && (retryCount--) > 0)
      {
        Thread.Sleep(duration);
      }

      if (!hasExited)
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
