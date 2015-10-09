
namespace Siftan
{
  using System;
  using System.Collections.Generic;

  public interface IRecordReader
  {
    #region Methods
    Record ReadRecord();
    #endregion
  }

  public class Record
  {
    public Int64 Start, End;
  }

  public interface IRecordWriter
  {
    void WriteRecord(IRecord record);
  }

  public interface IRecordDescriptor
  { 
  }

  public interface IRecord
  {
    //Boolean IsMatch(IRecordMatchExpression expression);
  }

  public interface IRecordMatch
  {
    #region Methods
    Boolean IsMatch(IRecord record);
    #endregion
  }

  public interface IRecordMatchComponent : IRecordMatch
  {
  }

  public interface IRecordMatchExpression : IRecordMatch
  {
  }
}
