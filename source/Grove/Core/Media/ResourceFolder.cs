namespace Grove.Media
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Ionic.Zip;

  public class ResourceFolder
  {
    private readonly string _directory;
    private readonly string _zipFile;

    public ResourceFolder(string name)
    {
      _directory = Path.Combine(MediaLibrary.BasePath, name);
      _zipFile = Path.Combine(MediaLibrary.BasePath, name + ".zip");
    }

    public bool HasDirectory { get { return Directory.Exists(_directory); } }
    public bool HasZip { get { return File.Exists(_zipFile); } }

    private string GetFullPath(string filename)
    {
      return Path.Combine(_directory, filename);
    }

    private Resource ReadFromDirectory(string filename)
    {
      var fullPath = GetFullPath(filename);
      return new Resource(filename, File.ReadAllBytes(fullPath), new FileInfo(fullPath).LastWriteTime);
    }
    
    private Resource ReadFromZip(string filename)
    {
      using (var file = ZipFile.Read(_zipFile))
      {
        using (var stream = new MemoryStream())
        {
          var zipEntry = file[filename];
          zipEntry.Extract(stream);
          return new Resource(filename, stream.ToArray(), zipEntry.LastModified);
        }
      }
    }

    private IEnumerable<Resource> ReadAllFromZip()
    {
      using (var file = ZipFile.Read(_zipFile))
      {
        foreach (var zipEntry in file.Entries)
        {
          using (var stream = new MemoryStream())
          {
            zipEntry.Extract(stream);
            yield return new Resource(zipEntry.FileName, stream.ToArray(),
              zipEntry.LastModified);
          }
        }
      }
    }

    private IEnumerable<Resource> ReadAllFromDirectory()
    {
      foreach (var path in Directory.EnumerateFiles(_directory))
      {
        yield return new Resource(
          Path.GetFileName(path),
          File.ReadAllBytes(path),
          new FileInfo(path).LastWriteTime);
      }
    }

    public IEnumerable<Resource> ReadAll()
    {
      if (HasZip)
        return ReadAllFromZip();

      if (HasDirectory)
        return ReadAllFromDirectory();

      return Enumerable.Empty<Resource>();
    }  
    
    public Resource ReadFile(string filename)
    {
      if (HasZip)
        return ReadFromZip(filename);

      return ReadFromDirectory(filename);
    }

    public void WriteFile(string filename, byte[] data)
    {
      if (HasZip)
      {
        AddToZip(filename, data);
        return;
      }

      AddToDirectory(filename, data);
    }

    private void AddToDirectory(string name, byte[] data)
    {
      File.WriteAllBytes(GetFullPath(name), data);
    }

    private void AddToZip(string name, byte[] data)
    {
      using (var file = new ZipFile(_zipFile))
      {
        file.AddEntry(name, data);
        file.Save();
      }
    }

    private bool HasZipEntry(string filename)
    {
      using (var file = new ZipFile(_zipFile))
      {
        return file.ContainsEntry(filename);
      }
    }

    private IEnumerable<string> GetZipEntries()
    {
      using (var file = new ZipFile(_zipFile))
      {
        return file.Entries.Select(x => x.FileName);
      }
    }

    private IEnumerable<string> GetFilesInDirectory()
    {
      return Directory.EnumerateFiles(_directory).Select(Path.GetFileName);
    }

    private bool HasFile(string filename)
    {
      return File.Exists(GetFullPath(filename));
    }

    public static implicit operator ResourceFolder(string name)
    {
      return new ResourceFolder(name);
    }

    public bool Has(string filename)
    {
      if (HasZip)
        return HasZipEntry(filename);

      return HasFile(filename);
    }

    public IEnumerable<string> GetFilenames()
    {
      if (HasZip)
        return GetZipEntries();

      return GetFilesInDirectory();
    }

    public long GetSize()
    {
      if (HasZip)
        return GetZipSize();

      if (HasDirectory)
        return GetDirectorySize();

      return 0;
    }

    private long GetDirectorySize()
    {
      return Directory.EnumerateFiles(_directory)
        .Sum(filename => new FileInfo(filename).Length);
    }

    private long GetZipSize()
    {
      using (var file = new ZipFile(_zipFile))
      {
        return file.Entries.Sum(x => x.UncompressedSize);
      }
    }
  }
}