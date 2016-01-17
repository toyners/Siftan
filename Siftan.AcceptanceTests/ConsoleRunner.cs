
namespace Siftan.AcceptanceTests
{
  using System;
  using System.Diagnostics;
  using System.Threading;
  using TestStack.White;

  public static class ConsoleRunner
  {
    public static void Run(String command, String commandArguments)
    {
      const Int32 oneSecond = 1000; // in milliseconds
      const Int32 thirtySeconds = 30;

      Stopwatch stopWatch = new Stopwatch();

      ProcessStartInfo processStartInfo = new ProcessStartInfo(command, commandArguments);

      stopWatch.Start();
      Application application = Application.Launch(processStartInfo);
      while (!application.Process.HasExited && stopWatch.Elapsed.Seconds < thirtySeconds)
      {
        Thread.Sleep(oneSecond);
      }

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
