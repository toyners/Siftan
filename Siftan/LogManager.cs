
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.Validation;

  public class LogManager : ILogManager, IDisposable
  {
    private Boolean disposedValue = false; // To detect redundant calls

    private StreamWriter applicationLog;

    private StreamWriter jobLog;

    public LogManager(String applicationLogFilePath, String jobLogFilePath)
    {
      applicationLogFilePath.VerifyThatStringIsNotNullAndNotEmpty("Parameter 'applicationLogFilePath' is null or empty.");
      jobLogFilePath.VerifyThatStringIsNotNullAndNotEmpty("Parameter 'jobLogFilePath' is null or empty.");

      FileStream applicationLogStream = new FileStream(applicationLogFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
      this.applicationLog = new StreamWriter(applicationLogStream);

      FileStream jobLogStream = new FileStream(jobLogFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
      this.jobLog = new StreamWriter(jobLogStream);
    }

    public void Close()
    {
      this.Dispose(true);
    }

    public void WriteMessage(LogEntryTypes logEntryType, String message, LogEntryFlushTypes flushType = LogEntryFlushTypes.Lazy)
    {
      if (logEntryType == LogEntryTypes.Application)
      {
        this.applicationLog.WriteLine(message);

        if (flushType == LogEntryFlushTypes.Force)
        {
          this.applicationLog.Flush();
        }

        return;
      }

      this.jobLog.WriteLine(message);
      if (flushType == LogEntryFlushTypes.Force)
      {
        this.jobLog.Flush();
      }
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      this.Dispose(true);
    }

    protected virtual void Dispose(Boolean disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          if (this.applicationLog != null)
          {
            this.applicationLog.Close();
            this.applicationLog = null;
          }

          if (this.jobLog != null)
          {
            this.jobLog.Close();
            this.jobLog = null;
          }
        }

        disposedValue = true;
      }
    }
  }
}
