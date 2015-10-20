
namespace Siftan
{
  public class Engine
  {
    #region Methods
    public void Execute(IRecordReader recordReader, IRecordMatchExpression expression, WriteRecordDelegate writeMatchedRecord, WriteRecordDelegate writeUnmatchedRecord)
    {
      Record record;
      while (!expression.HasReachedMatchQuota && (record = recordReader.ReadRecord()) != null)
      {
        if (expression.IsMatch(record))
        {
          writeMatchedRecord(record);
        }
        else if (writeUnmatchedRecord != null)
        {
          writeUnmatchedRecord(record);
        }
      }
    }
    #endregion
  }
}
