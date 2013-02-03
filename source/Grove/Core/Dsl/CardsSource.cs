namespace Grove.Core.Dsl
{
  using System.Collections.Generic;
  using Effects;  

  public abstract class CardsSource
  {
    public CardFactory Card { get { return new CardFactory(); } }
    public abstract IEnumerable<CardFactory> GetCards();

    protected T[] L<T>(params T[] elt)
    {
      return elt;
    }

    protected EffectChoice Choice(params EffectChoiceOption[] options)
    {
      return new EffectChoice(options);
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