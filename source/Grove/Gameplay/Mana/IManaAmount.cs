namespace Grove.Gameplay.Mana
{
  using System.Collections.Generic;

  public interface IManaAmount : IEnumerable<SingleColorManaAmount>
  {
    int Converted { get; }
    
    IManaAmount Add(IManaAmount amount);
    IManaAmount Remove(IManaAmount amount);
  }
}