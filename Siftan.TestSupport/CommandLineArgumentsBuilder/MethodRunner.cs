
namespace Siftan.TestSupport
{
  using System;
  using System.Diagnostics;
  using System.Threading;

  public static class MethodRunner
  {
    public static void RunForDuration(Func<Boolean> action)
    {
      const Int32 oneSecond = 1000; // in milliseconds
      const Int32 thirtySeconds = 30;
      Stopwatch stopWatch = new Stopwatch();
      stopWatch.Start();

      while (!action() && stopWatch.Elapsed.Seconds < thirtySeconds)
      {
        Thread.Sleep(oneSecond);
      }
    }
  }
}
