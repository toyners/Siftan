
namespace Siftan
{
  using System;

  public class DelimitedRecordDescriptor
  {
    public String Delimiter;

    public Char Qualifier;

    public String HeaderID;

    public UInt32 LineIDIndex;

    public TermDefinition DelimitedTerm;

    public struct TermDefinition
    {
      public readonly String LineID;

      public readonly UInt32 Index;

      public TermDefinition(String lineID, UInt32 index)
      {
        LineID = lineID;
        Index = index;
      }
    }
  }
}
