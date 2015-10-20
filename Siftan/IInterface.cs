
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

  public interface IRecordMatchExpression
  {
    Boolean IsMatch(Record record);

    Boolean HasReachedMatchQuota { get; }

    event WriteRecordDelegate WriteRecordToFile;
  }

  public class RecordMatchExpression
  {
    public RecordMatchExpression()
    {
 
    }

    public event WriteRecordDelegate WriteRecordToFile;

    public virtual Boolean IsMatch(Record record)
    {
      return false;
    }

    public virtual Boolean HasReachedMatchQuota { get; protected set; }


  }
}
