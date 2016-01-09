﻿
namespace Siftan
{
  using System;

  public interface ILogManager
  {
    void WriteMessage(LogEntryTypes logEntryType, String message, LogEntryFlushTypes flushType = LogEntryFlushTypes.Lazy);
  }
}
