namespace Grove.Core.CardDsl
{
  using System.Collections.Generic;
  using Ai;

  public delegate void Initializer<in T>(T target, CardCreationCtx creationContext);

  public abstract class CardsSource
  {
    public CardCreationCtx C { get { return new CardCreationCtx(Game); } }

    public Game Game { get; set; }

    public abstract IEnumerable<ICardFactory> GetCards();

    protected TimingDelegate All(params TimingDelegate[] predicates)
    {
      return Timings.All(predicates);
    }

    protected TimingDelegate Any(params TimingDelegate[] predicates)
    {
      return Timings.Any(predicates);
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