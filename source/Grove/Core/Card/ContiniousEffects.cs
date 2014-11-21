namespace Grove
{
  using Infrastructure;
  using Modifiers;

  public class ContiniousEffects : GameObject, IAcceptsPlayerModifier
  {
    private readonly TrackableList<ContinuousEffect> _continiousEffects = new TrackableList<ContinuousEffect>();

    public void Accept(IPlayerModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(Card source, Game game)
    {
      Game = game;

      _continiousEffects.Initialize(ChangeTracker);
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