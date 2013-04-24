namespace Grove.Gameplay.Card.Abilities
{
  using Grove.Infrastructure;
  using Modifiers;

  [Copyable]
  public class ContiniousEffects : IModifiable
  {
    private readonly TrackableList<ContinuousEffect> _continiousEffects = new TrackableList<ContinuousEffect>();

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(Card owner, Game game, IHashDependancy hashDependancy)
    {
      _continiousEffects.Initialize(game.ChangeTracker, hashDependancy);

      foreach (var continiousEffect in _continiousEffects)
      {
        continiousEffect.Initialize(owner, game);
      }
    }

    public void Add(ContinuousEffect effect)
    {
      _continiousEffects.Add(effect);
    }

    public void Remove(ContinuousEffect effect)
    {
      _continiousEffects.Remove(effect);
      effect.Deactivate();
    }    
  }
}