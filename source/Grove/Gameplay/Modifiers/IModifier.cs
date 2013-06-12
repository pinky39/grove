namespace Grove.Gameplay.Modifiers
{
  using System;

  public interface IModifier : IDisposable
  {
    IModifiable Owner { get; }
    
    void Activate();
    void Initialize(ModifierParameters p, Game game);
    void AddLifetime(Lifetime lifetime);
  }
}