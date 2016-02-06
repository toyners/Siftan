
namespace Siftan
{
  using System;

  public class FixedWidthRecordDescriptor
  {
    #region Fields
    public UInt32 LineIDStart;

    public UInt32 LineIDLength;

    public String HeaderID;

    public TermDefinition Term;
    #endregion

    #region Structs
    public struct TermDefinition
    {
      public String LineID;

      public UInt32 Start;

      public UInt32 Length;

      public TermDefinition(String lineID, UInt32 start, UInt32 length)
      {
        this.LineID = lineID;
        this.Start = start;
        this.Length = length;
      }
    }
    #endregion
  }
}
