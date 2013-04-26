namespace Grove.Gameplay.Card.Factory
{
  using System;
  using System.Collections.Generic;
  using Abilities;
  using Effects;
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

    protected DynParam<T> P<T>(Func<Effect, Game, T> getter, bool evaluateOnInit = true, bool evaluateOnResolve = false)
    {
      return new DynParam<T>(getter, evaluateOnInit, evaluateOnResolve);
    }

    protected DynParam<T> P<T>(Func<Effect, T> getter, bool evaluateOnInit = true, bool evaluateOnResolve = false)
    {
      return new DynParam<T>((e, g) => getter(e), evaluateOnInit, evaluateOnResolve);
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