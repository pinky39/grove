namespace Grove.Gameplay
{
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;

  public abstract class CastingRule : GameObject
  {
    protected Card Card { get; private set; }

    public abstract bool CanCast();

    public virtual void Cast(Effect effect)
    {            
      Publish(new BeforeSpellWasPutOnStack(
        effect.Source.OwningCard, 
        effect.Targets));      
      
      Stack.Push(effect);      
      
      Publish(new AfterSpellWasPutOnStack(
        effect.Source.OwningCard, 
        effect.Targets));
    }

    public abstract void AfterResolve();


    public virtual void Initialize(Card card, Game game)
    {
      Game = game;
      Card = card;
    }
  }
}