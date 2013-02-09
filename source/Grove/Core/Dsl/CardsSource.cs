namespace Grove.Core.Dsl
{
  using System.Collections.Generic;
  using Modifiers;

  public abstract class CardsSource
  {
    public CardFactory Card { get { return new CardFactory(); } }
    public abstract IEnumerable<CardFactory> GetCards();

    protected ModifierFactory[] L(params ModifierFactory[] elt)
    {
      return elt;
    }
    
    protected T[] L<T>(params T[] elt)
    {
      return elt;
    }

    public LevelDefinition Level(int min, int power, int toughness, Static ability, int? max = null)
    {
      return new LevelDefinition
        {
          Min = min,
          Max = max,
          Power = power,
          Toughness = toughness,
          StaticAbility = ability
        };
    }
  }
}