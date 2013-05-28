namespace Grove.Infrastructure
{
  using System;
  using System.Linq;
  using System.Collections.Generic;
  using System.IO;
  using System.Runtime.Serialization;
  using System.Runtime.Serialization.Formatters.Binary;

  public class SerializationContext
  {
    public SerializationContext(IEnumerable<object> singletons)
    {
      Singletons = singletons.ToDictionary(x => x.GetType(), x => x);
    }
    
    public Dictionary<Type, object> Singletons { get; private set; }
  }

  public class SingletonSerializationHelper<T> : IObjectReference
  {
    public object GetRealObject(StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;
      return ctx.Singletons[typeof (T)];
    }
  }

  public class CopyService2
  {
    public static object Copy(object obj, SerializationContext serializationContext = null)
    {
      var stream = new MemoryStream();

      var formatter = new BinaryFormatter
        {
          Context = new StreamingContext(StreamingContextStates.Clone, serializationContext)
        };

      formatter.Serialize(stream, obj);
      stream.Position = 0;


      return formatter.Deserialize(stream);
    }
  }
}