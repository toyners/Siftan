
namespace Siftan
{
  using System;
  using System.Linq;
  using Jabberwocky.Toolkit.Validation;

  public class InListExpression : IRecordMatchExpression
  {
    #region Enums
    public enum MatchQuotas
    {
      None,
      FirstMatchInList,
      FirstMatchOfEachTermInList
    }
    #endregion

    #region Fields
    private readonly ValueRecord[] values;

    private readonly Boolean[] valuesMatched;

    private readonly MatchQuotas matchQuota;
    #endregion

    #region Construction
    public InListExpression(String[] values, MatchQuotas matchQuota = MatchQuotas.None)
    {
      values.VerifyThatArrayIsNotNullAndNotEmpty("Parameter 'values' is null or empty.");

      this.values = new ValueRecord[values.Length];
      for (Int32 index = 0; index < values.Length; index++)
      {
        this.values[index].Value = values[index];
      }

      this.matchQuota = matchQuota;

      if (matchQuota == MatchQuotas.FirstMatchOfEachTermInList)
      {
        this.valuesMatched = new Boolean[values.Length];
      }
    }
    #endregion

    #region Properties
    public Boolean HasReachedMatchQuota { get; protected set; }
    #endregion

    #region Events
    public event WriteRecordDelegate WriteRecordToFile;
    #endregion

    #region Methods
    public Boolean IsMatch(Record record)
    {
      Boolean found = false;
      for (Int32 index = 0; index < this.values.Length; index++)
      {
        if (this.values[index].Value == record.Term)
        {
          this.values[index].MatchedCount++;
          found = true;
          break;
        }
      }

      if (!found)
      {
        return false;
      }

      this.WriteMatchedRecord(record);

      if (this.matchQuota == MatchQuotas.FirstMatchInList)
      {
        this.HasReachedMatchQuota = true;
      }

      if (this.matchQuota == MatchQuotas.FirstMatchOfEachTermInList && !this.values.Any(x => x.MatchedCount == 0))
      {
        this.HasReachedMatchQuota = true;
      }

      return true;
    }

    private void WriteMatchedRecord(Record record)
    {
      if (this.WriteRecordToFile!= null)
      {
        this.WriteRecordToFile(record);
      }
    }
    #endregion

    #region Structs
    private struct ValueRecord
    {
      public String Value;

      public Int32 MatchedCount;
    }
    #endregion
  }
}
