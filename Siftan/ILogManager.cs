
namespace Siftan
{
  using System;

  public interface ILogManager
  {
    void WriteMessageToApplicationLog(String message);

    void WriteMessageToJobLog(String message);
  }
}
