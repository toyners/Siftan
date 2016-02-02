
namespace Siftan.WinForms
{
  using System;

  public class UILogManager : ILogManager
  {
    private readonly ILogManager logManager;

    public event MessageLoggedEventHandler MessageLogged;

    public UILogManager(ILogManager logManager)
    {
      this.logManager = logManager;
    }

    public String JobLogFilePath
    {
      get { return this.logManager.JobLogFilePath; }
      set { this.logManager.JobLogFilePath = value; }
    }

    public void Close()
    {
      this.logManager.Close();
    }

    public void WriteMessagesToLogs(String message)
    {
      this.WriteMessageToApplicationLog(message);
      this.WriteImportantMessageToJobLog(message);
    }

    public void WriteMessageToApplicationLog(String message)
    {
      this.logManager.WriteMessageToApplicationLog(message);
    }

    public void WriteImportantMessageToJobLog(String message)
    {
      this.logManager.WriteMessageToJobLog(message);
      this.OnMessageLogged(message);
    }

    public void WriteMessageToJobLog(String message)
    {
      this.logManager.WriteMessageToJobLog(message);
    }

    private void OnMessageLogged(String message)
    {
      if (this.MessageLogged != null)
      {
        this.MessageLogged(this, message);
      }
    }
  }
}
