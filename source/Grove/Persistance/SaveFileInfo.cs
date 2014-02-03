namespace Grove.Persistance
{
  using System;

  [Serializable]
  public class SaveFileInfo
  {
    public readonly string Name;
    public readonly string Description;
    public readonly DateTime CreatedAt;
    
    public SaveFileInfo(string name, string description, DateTime createdAt)
    {
      Name = name;
      Description = description;
      CreatedAt = createdAt;
    }
  }
}