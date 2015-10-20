
namespace Siftan
{
  using System;

  public interface IRecordMatchExpression
  {
    #region Properties
    Boolean HasReachedMatchQuota { get; }
    #endregion

    #region Events
    event WriteRecordDelegate WriteRecordToFile;
    #endregion

    #region Methods
    Boolean IsMatch(Record record);
    #endregion
  }
}
