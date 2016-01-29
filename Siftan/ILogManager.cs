
namespace Siftan
{
  using System;

  public interface ILogManager
  {
    void WriteMessagesToLogs(String message);

    void WriteMessageToApplicationLog(String message);

    void WriteMessageToJobLog(String message);

    void WriteStatisticsToJobLog(IStatisticsCollector statisticsCollector);
  }
}
