
namespace Siftan
{
  using System;

  public class DateTimeStamper : IDateTimeStamper
  {
    public String Now
    {
      get
      {
        return DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
      }
    }
  }
}
