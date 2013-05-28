namespace Grove.Gameplay.CastingRules
{
  using System;
  using Effects;
  using Misc;

  [Serializable]
  public abstract class CastingRule : GameObject
  {
    protected Card Card { get; private set; }

    public abstract bool CanCast();
    public abstract void Cast(Effect effect);
    public abstract void AfterResolve();

    public virtual void Initialize(Card card, Game game)
    {
      Game = game;
      Card = card;
    }
  }
}