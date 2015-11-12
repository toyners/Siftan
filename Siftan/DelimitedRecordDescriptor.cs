
namespace Siftan
{
  using System;

  public class DelimitedRecordDescriptor
  {
    public String Delimiter;

    public Char Qualifier;

    public String HeaderID;

    public UInt32 LineIDIndex;

    public SearchDefinition[] SearchDefinitions;

    public struct SearchDefinition
    {
      String LineID;

      Int32 Index;
    }
  }
}
