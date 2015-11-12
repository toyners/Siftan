
namespace Siftan
{
  using System;

  public interface IRecordMatchExpression
  {
    #region Properties
    Boolean HasReachedMatchQuota { get; }
    #endregion

    #region Methods
    Boolean IsMatch(Record record);
    #endregion
  }
}
