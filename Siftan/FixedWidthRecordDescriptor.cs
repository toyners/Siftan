
namespace Siftan
{
  using System;

  public class FixedWidthRecordDescriptor
  {
    #region Fields
    public readonly UInt32 LineIDStart;

    public readonly UInt32 LineIDLength;

    public readonly String HeaderID;

    public readonly TermDefinition Term;
    #endregion

    #region Construction
    public FixedWidthRecordDescriptor(UInt32 lineIDStart, UInt32 lineIDLength, String headerID, TermDefinition term)
    {
      this.LineIDStart = lineIDStart;
      this.LineIDLength = lineIDLength;
      this.HeaderID = headerID;
      this.Term = term;
    }
    #endregion

    #region Structs
    public struct TermDefinition
    {
      public readonly String LineID;

      public readonly UInt32 Start;

      public readonly UInt32 Length;

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
