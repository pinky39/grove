namespace Grove.Persistance
{
  using System;

  public class SaveGameFile
  {
    public readonly object Data;
    public readonly DateTime? CreatedAt;
    public readonly SaveFileHeader Header;

    public SaveGameFile(SaveFileHeader header, object data, DateTime? createdAt)
    {
      Header = header;
      Data = data;
      CreatedAt = createdAt;
    }
  }
}