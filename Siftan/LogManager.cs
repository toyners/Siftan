
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.Object;
  using Jabberwocky.Toolkit.String;

  public class LogManager : ILogManager, IDisposable
  {
    private Boolean disposedValue = false; // To detect redundant calls

    private StreamWriter applicationLog;

    private StreamWriter jobLog;

    private String jobLogFilePath;

    private IDateTimeStamper dateTimeStamper;

    public String JobLogFilePath
    {
      get { return this.jobLogFilePath; }
      set
      {
        value.VerifyThatStringIsNotNullAndNotEmpty("Property 'JobLogFilePath' is being set to null or empty.");
        this.jobLogFilePath = value;
      }
    }

    public Boolean JobLogIsClosed { get { return this.jobLog == null; } }

    public LogManager(String applicationLogFilePath)
      : this(new DateTimeStamper(), applicationLogFilePath)
    {
    }

    public LogManager(IDateTimeStamper dateTimeStamper, String applicationLogFilePath)
    {
      dateTimeStamper.VerifyThatObjectIsNotNull("Parameter 'dateTimeStamper' is null.");
      applicationLogFilePath.VerifyThatStringIsNotNullAndNotEmpty("Parameter 'applicationLogFilePath' is null or empty.");

      this.dateTimeStamper = dateTimeStamper;

      FileStream applicationLogStream = new FileStream(applicationLogFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
      this.applicationLog = new StreamWriter(applicationLogStream);
      this.applicationLog.AutoFlush = true;
    }

    public void Close()
    {
      this.Dispose(true);
    }

    public void WriteMessageToApplicationLog(String message)
    {
      this.applicationLog.WriteLine(this.dateTimeStamper.Now + " " + message);
    }

    public void WriteMessageToJobLog(String message)
    {
      if (this.JobLogIsClosed)
      {
        this.OpenJobLog();
      }

      this.jobLog.WriteLine(this.dateTimeStamper.Now + " " + message);
    }

    private void OpenJobLog()
    {
      FileStream jobLogStream = new FileStream(this.jobLogFilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
      this.jobLog = new StreamWriter(jobLogStream);
      this.jobLog.AutoFlush = true;
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing).
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

          if (!this.JobLogIsClosed)
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
