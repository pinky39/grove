namespace Grove.Persistance
{
  using System;
  using System.Collections;
  using System.IO;
  using System.Runtime.Serialization;

  [Serializable]
  public class SaveFileHeader : ISerializable
  {
    private readonly Hashtable _data = new Hashtable();

    public SaveFileHeader()
    {
      Version = 2;
    }

    public SaveFileHeader(SerializationInfo info, StreamingContext context)
    {
      try
      {
        _data = (Hashtable) info.GetValue("data", typeof (Hashtable));
      }
      catch (SerializationException)
      {
        // Version 1 
        // Only description field was available!
        var description = info.GetString("Description");

        _data.Add("description", description);
        _data.Add("version", 1);
      }
    }

    public string Description { get { return (string) _data["description"]; } set { _data["description"] = value; } }
    public int Version { get { return (int) _data["version"]; } set { _data["version"] = value; } }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("data", _data);
    }
  }
}