namespace Grove.Core.Dsl
{
  using System.Collections.Generic;
  using Ai;
  using Details.Cards;

  public delegate void Initializer<in T>(T target, CardBuilder builder);

  public abstract class CardsSource
  {
    public CardBuilder C { get { return new CardBuilder(Game); } }

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

    protected TargetsFilterDelegate Any(params TargetsFilterDelegate[] delegates)
    {
      return TargetFilters.Any(delegates);
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
          Thoughness = toughness,
          StaticAbility = ability
        };
    }
  }
}