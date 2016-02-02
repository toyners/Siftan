
namespace Siftan
{
  using System;

  public interface ILogManager
  {
    String JobLogFilePath { get; set; }

    void WriteMessagesToLogs(String message);

    void WriteMessageToApplicationLog(String message);

    void WriteImportantMessageToJobLog(String message);

    void WriteMessageToJobLog(String message);

    void Close();
  }
}
