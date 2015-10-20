
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  
  public interface IRecordWriter
  {
    void WriteRecord(Record record);
  }

  public delegate void WriteRecordDelegate(Record record);
}
