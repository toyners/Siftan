
namespace Siftan
{
  using System;

  public class Record
  {
    #region Fields
    public Int64 Start;

    // End position of the record. This is not inclusive i.e. the last character is End - 1
    public Int64 End;

    public String Term;
    #endregion
  }
}
