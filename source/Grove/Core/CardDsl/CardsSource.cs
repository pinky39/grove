namespace Grove.Core.CardDsl
{
  using System.Collections.Generic;
  using Ai;

  public delegate void Initializer<in T>(T target, CardCreationContext creationContext);

  public abstract class CardsSource
  {
    public CardCreationContext C { get { return new CardCreationContext(Game); } }

    public Game Game { get; set; }

    public abstract IEnumerable<ICardFactory> GetCards();

    protected TimingDelegate All(params TimingDelegate[] predicates)
    {
      return Timings.NoRestrictions(predicates);
    }

    protected TimingDelegate Any(params TimingDelegate[] predicates)
    {
      return Timings.Any(predicates);
    }

    protected TargetsFilterDelegate Any(params  TargetsFilterDelegate[] delegates)
    {
      return TargetFilters.Any(delegates);
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