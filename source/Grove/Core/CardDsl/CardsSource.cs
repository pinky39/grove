namespace Grove.Core.CardDsl
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public delegate void Initializer<in T>(T target, CardCreationCtx creationContext);

  public abstract class CardsSource
  {
    public CardCreationCtx C
    {
      get { return new CardCreationCtx(Game); }
    }

    public Game Game { get; set; }

    public abstract IEnumerable<ICardFactory> GetCards();

    protected Func<Game, Card, ActivationParameters, bool> All(
      params Func<Game, Card, ActivationParameters, bool>[] predicates)
    {
      return
        (game, card, activationParameters) => predicates.All(predicate => predicate(game, card, activationParameters));
    }

    protected Func<Game, Card, ActivationParameters, bool> Any(
      params Func<Game, Card, ActivationParameters, bool>[] predicates)
    {
      return
        (game, card, activationParameters) => predicates.Any(predicate => predicate(game, card, activationParameters));
    }

    protected T[] L<T>(params T[] elt)
    {
      return elt;
    }

    public LevelDefinition Level(int min, int power, int toughness, StaticAbility ability, int? max = null)
    {
      return new LevelDefinition
        {
          Min = min,
          Max = max,
          Power = power,
          Thoughness = toughness,
          StaticAbility = ability
        };
    }
  }
}