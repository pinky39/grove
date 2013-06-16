namespace Grove.Persistance
{
  using System.IO;
  using System.Runtime.Serialization.Formatters;
  using System.Runtime.Serialization.Formatters.Binary;
  using UserInterface;

  public static class SaveLoadHelper
  {
    public static void WriteToDisk(SaveFileHeader header, object saveGameData, string filename = null)
    {
      filename = filename ?? MediaLibrary.GetSaveGameFilename();
      var formatter = CreateFormatter();

      using (var file = new FileStream(filename, FileMode.Create))
      {
        formatter.Serialize(file, header);
        formatter.Serialize(file, saveGameData);
      }
    }

    public static BinaryFormatter CreateFormatter()
    {
      var formatter = new BinaryFormatter();
      formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
      return formatter;
    }

    public static SaveFileHeader ReadHeader(string filename)
    {
      var formatter = CreateFormatter();

      using (var file = new FileStream(filename, FileMode.Open))
      {
        var header = (SaveFileHeader) formatter.Deserialize(file);
        return header;
      }
    }

    public static object ReadData(string filename)
    {
      var formatter = CreateFormatter();

      using (var file = new FileStream(filename, FileMode.Open))
      {
        formatter.Deserialize(file);
        return formatter.Deserialize(file);
      }
    }   
  }
}