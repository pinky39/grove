namespace Grove.Persistance
{
  using System;

  public class Resource
  {
    public readonly byte[] Content;
    public readonly DateTime ModifiedAt;
    public readonly string Name;

    public Resource(string name, byte[] content, DateTime modifiedAt)
    {
      Content = content;
      Name = name;
      ModifiedAt = modifiedAt;
    }
  }
}