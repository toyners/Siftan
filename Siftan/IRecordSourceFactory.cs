
namespace Siftan
{
  using System;

  public interface IRecordSourceFactory
  {
    IRecordSource CreateSource(String key);
  }
}
