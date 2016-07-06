
namespace Siftan.Console
{
  using System;
  using System.Collections.Generic;

  public static class QueueExtensions
  {
    public static String DequeueArgument(this Queue<String> queue, String field = null)
    {
      if (queue.Count == 0)
      {
        if (field != null)
        {
          throw new Exception(String.Format("Missing value for field '{0}'.", field));
        }

        throw new Exception("No arguments found.");
      }

      return queue.Dequeue();
    }

    public static UInt32 DequeueUInt32(this Queue<String> queue, String field)
    {
      var value = DequeueArgument(queue, field);
      return ConvertToUInt32(value);
    }

    public static Char DequeueChar(this Queue<String> queue, String field)
    {
      var value = DequeueArgument(queue, field);
      if (value.Length > 1)
      {
        throw new InvalidCastException(String.Format("Value '{0}' cannot be cast to type Char.", value));
      }

      return value[0];
    }

    public static T DequeueEnum<T>(this Queue<String> queue, String field) where T : struct
    {
      var value = DequeueArgument(queue, field);

      T result;
      if (!Enum.TryParse(value, out result))
      {
        throw new Exception(String.Format("Value '{0}' cannot be cast to type {1}.", value, result.GetType()));
      }

      return result;
    }

    public static String[] DequeueArray(this Queue<String> queue, Char seperator, String field)
    {
      var value = DequeueArgument(queue, field);
      return value.Split(seperator);
    }

    private static UInt32 ConvertToUInt32(String value)
    {
      UInt32 convertedValue;
      if (!UInt32.TryParse(value, out convertedValue))
      {
        throw new InvalidCastException(String.Format("Value '{0}' cannot be cast to type UInt32.", value));
      }

      return convertedValue;
    }
  }
}
