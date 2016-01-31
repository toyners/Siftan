
namespace Siftan.Console
{
  using System;
  using System.Collections;

  internal static class QueueOperations
  {
    internal static String DequeueArgument(Queue queue, String field = null)
    {
      if (queue.Count == 0)
      {
        if (field != null)
        {
          throw new Exception(String.Format("Missing value for field '{0}'.", field));
        }
      }

      return queue.Dequeue() as String;
    }

    internal static UInt32 DequeueUInt32(Queue queue, String field)
    {
      String value = DequeueArgument(queue, field);
      return ConvertToUInt32(value);
    }

    internal static Char DequeueChar(Queue queue, String field)
    {
      String value = DequeueArgument(queue, field);
      if (value.Length > 1)
      {
        throw new InvalidCastException(String.Format("Value '{0}' cannot be cast to type Char.", value));
      }

      return value[0];
    }

    internal static UInt32 ConvertToUInt32(String value)
    {
      UInt32 convertedValue;
      if (!UInt32.TryParse(value, out convertedValue))
      {
        throw new InvalidCastException(String.Format("Value '{0}' cannot be cast to type UInt32.", value));
      }

      return convertedValue;
    }

    internal static T DequeueEnum<T>(Queue queue, String field) where T : struct
    {
      String value = DequeueArgument(queue, field);

      T result;
      if (!Enum.TryParse(value, out result))
      {
        throw new Exception(String.Format("Value '{0}' cannot be cast to type {1}.", value, result.GetType().ToString()));
      }

      return result;
    }

    internal static String[] DequeueArray(Queue queue, Char seperator, String field)
    {
      String value = DequeueArgument(queue, field);
      return value.Split(seperator);
    }
  }
}
