namespace Grove.Infrastructure
{
  using System;

  [Serializable]
  public class NullHashDependency : IHashDependancy
  {
    public void InvalidateHash() {}
  }
}