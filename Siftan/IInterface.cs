
namespace Siftan
{
  using System;
  using System.Collections.Generic;

  public interface IRecordReader
  {
    #region Methods
    IRecord ReadRecord();
    #endregion
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
