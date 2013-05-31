namespace Grove.Persistance
{
  using System.IO;
  using System.Runtime.Serialization;
  using System.Runtime.Serialization.Formatters.Binary;
  using Gameplay;

  public class DecisionLog
  {
    private readonly BinaryFormatter _formatter = new BinaryFormatter();
    private readonly MemoryStream _stream;

    public DecisionLog(Game game)
    {
       _stream = new MemoryStream();
      
      _formatter.Context = new StreamingContext(
        StreamingContextStates.All,
        new SerializationContext {Game = game});
    }

    public void SaveResult(object result)
    {
      _formatter.Serialize(_stream, result);
    }

    public object LoadResult()
    {
      return _formatter.Deserialize(_stream);
    }
  }
}