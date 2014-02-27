namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.Serialization.Formatters;
  using System.Runtime.Serialization.Formatters.Binary;
  using Grove.Media;

  public static class SavedGames
  {
    private const string IndexFile = "index";
    private static readonly ResourceFolder Folder = "saved";

    public static void Write(SaveFileHeader header, object data)
    {
      using (var stream = new MemoryStream())
      {
        WriteToStream(header, data, stream);
        var filename = String.Format("{0}.savegame", Guid.NewGuid());

        Folder.WriteFile(filename, stream.ToArray());
      }
    }

    public static void WriteToStream(SaveFileHeader header, object data, Stream stream)
    {
      var formatter = CreateFormatter();
      formatter.Serialize(stream, header);
      formatter.Serialize(stream, data);
    }

    public static IEnumerable<SaveFileInfo> GetDescriptions()
    {
      var files = Folder.GetFilenames().Where(x => x.EndsWith(".savegame"));
      var index = Folder.Has(IndexFile) ? ReadIndex() : new SaveGameIndex();

      var indexModified = false;

      foreach (var filename in files)
      {
        if (!index.Entries.ContainsKey(filename))
        {
          var saveGameFile = Read(filename);
          var info = new SaveFileInfo(filename, saveGameFile.Header.Description, saveGameFile.CreatedAt.Value);

          index.Entries[filename] = info;
          yield return info;

          indexModified = true;
        }
        else
        {
          yield return index.Entries[filename];
        }
      }

      if (indexModified)
        WriteIndex(index);
    }

    public static SaveGameFile ReadFromStream(Stream stream, DateTime? modifiedAt = null)
    {
      var formatter = CreateFormatter();
      var header = (SaveFileHeader) formatter.Deserialize(stream);
      var data = formatter.Deserialize(stream);

      return new SaveGameFile(header, data, modifiedAt);
    }

    public static SaveGameFile Read(string filename)
    {
      var file = Folder.ReadFile(filename);

      using (var stream = new MemoryStream(file.Content))
      {
        return ReadFromStream(stream, file.ModifiedAt);
      }
    }

    private static BinaryFormatter CreateFormatter()
    {
      return new BinaryFormatter
        {
          AssemblyFormat = FormatterAssemblyStyle.Simple,
          Binder = new RenameBinder()
        };
    }

    private static SaveGameIndex ReadIndex()
    {
      var formatter = CreateFormatter();
      var file = Folder.ReadFile(IndexFile);

      using (var stream = new MemoryStream(file.Content))
      {
        return (SaveGameIndex) formatter.Deserialize(stream);
      }
    }

    private static void WriteIndex(SaveGameIndex index)
    {
      var formatter = CreateFormatter();

      using (var stream = new MemoryStream())
      {
        formatter.Serialize(stream, index);
        Folder.WriteFile(IndexFile, stream.ToArray());
      }
    }
  }
}