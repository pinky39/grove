namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.Serialization.Formatters;
  using System.Runtime.Serialization.Formatters.Binary;
  using Grove.Infrastructure;
  using Grove.Media;

  public static class PregeneratedDecks
  {
    private const string IndexFile = "index";
    private static readonly ResourceFolder Folder = "tournament";

    public static List<Deck> GetRandom(int limitedCode, int count)
    {
      var all = GetFiles(limitedCode);
      var resultCount = Math.Min(count, all.Count);
      var result = new List<Deck>();

      var permutation = new RandomGenerator().GetRandomPermutation(0, all.Count);

      for (var i = 0; i < resultCount; i++)
      {
        result.Add(Get(all[permutation[i]]));
      }

      return result;
    }

    public static void Write(Deck deck)
    {
      var filename = String.Format("{0}.dec", Guid.NewGuid());
      Folder.WriteFile(filename, DeckFile.Write(deck));
    }

    private static List<string> GetFiles(int limitedCode)
    {
      var files = Folder.GetFilenames().Where(f => f.EndsWith(".dec"));
      var index = Folder.Has(IndexFile) ? ReadIndex() : BuildIndex();

      var indexModified = false;
      var result = new List<string>();

      foreach (var filename in files)
      {
        if (!index.Entries.ContainsKey(filename))
        {
          var deck = Get(filename);
          index.Entries[filename] = deck.LimitedCode.GetValueOrDefault();

          if (deck.LimitedCode == limitedCode)
            result.Add(filename);

          indexModified = true;
        }
        else
        {
          var code = index.Entries[filename];

          if (code == limitedCode)
            result.Add(filename);
        }
      }

      if (indexModified)
        WriteIndex(index);

      return result;
    }

    private static SealedDeckIndex BuildIndex()
    {
      var index = new SealedDeckIndex();

      foreach (var resource in Folder.ReadAll())
      {
        if (!resource.Name.EndsWith(".dec"))
          continue;
        
        var deck = DeckFile.Read(resource.Name, resource.Content);
        index.Entries[resource.Name] = deck.LimitedCode.GetValueOrDefault();
      }

      WriteIndex(index);      
      return index;
    }

    private static Deck Get(string filename)
    {
      var file = Folder.ReadFile(filename);
      return DeckFile.Read(filename, file.Content);
    }

    private static void WriteIndex(SealedDeckIndex index)
    {
      var formatter = CreateFormatter();

      using (var stream = new MemoryStream())
      {
        formatter.Serialize(stream, index);
        Folder.WriteFile(IndexFile, stream.ToArray());
      }
    }

    private static SealedDeckIndex ReadIndex()
    {
      var formatter = CreateFormatter();
      var file = Folder.ReadFile(IndexFile);

      using (var stream = new MemoryStream(file.Content))
      {
        return (SealedDeckIndex) formatter.Deserialize(stream);
      }
    }

    private static BinaryFormatter CreateFormatter()
    {
      return new BinaryFormatter
        {
          AssemblyFormat = FormatterAssemblyStyle.Simple
        };
    }
  }
}