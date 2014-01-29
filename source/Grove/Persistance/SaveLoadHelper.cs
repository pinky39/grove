namespace Grove.Persistance
{
  using System;
  using System.IO;
  using System.Runtime.Serialization.Formatters;
  using System.Runtime.Serialization.Formatters.Binary;

  public class SaveGameFile
  {
    public readonly object Data;    
    public readonly SaveFileHeader Header;
    public readonly DateTime ModifiedAt;
    public readonly string Name;

    public SaveGameFile(string name, SaveFileHeader header, object data, DateTime modifiedAt)
    {
      Header = header;
      Data = data;
      ModifiedAt = modifiedAt;
      Name = name;
    }
  }

  public static class SaveLoadHelper
  {
    public static byte[] Serialize(SaveFileHeader header, object saveGameData)
    {
      var formatter = CreateFormatter();

      using (var stream = new MemoryStream())
      {
        formatter.Serialize(stream, header);
        formatter.Serialize(stream, saveGameData);

        return stream.ToArray();
      }
    }

    public static BinaryFormatter CreateFormatter()
    {
      var formatter = new BinaryFormatter();
      formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
      return formatter;
    }

    public static SaveGameFile ReadFile(string name, Stream stream, DateTime modifiedAt)
    {
      var formatter = CreateFormatter();
      var header = (SaveFileHeader) formatter.Deserialize(stream);
      var data = formatter.Deserialize(stream);
      return new SaveGameFile(name, header, data, modifiedAt);
    }
  }
}