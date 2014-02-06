namespace Grove.Gameplay.CastingRules
{
  using Effects;
  using Messages;
  using Misc;

  public abstract class CastingRule : GameObject
  {
    protected Card Card { get; private set; }

    public abstract bool CanCast();

    public virtual void Cast(Effect effect)
    {            
      Stack.Push(effect);
      Publish(new PlayerHasCastASpell(effect.Source.OwningCard, effect.Targets));
    }

    public abstract void AfterResolve();


    public virtual void Initialize(Card card, Game game)
    {
      Game = game;
      Card = card;
    }
  }
}