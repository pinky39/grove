namespace Grove.Core.Dsl
{
  using System.Collections.Generic;
  using Ai;
  using Details.Cards;
  using Details.Cards.Effects;


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

    protected TargetSelectorAiDelegate Any(params TargetSelectorAiDelegate[] delegates)
    {
      return TargetSelectorAi.Any(delegates);
    }

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
          Thoughness = toughness,
          StaticAbility = ability
        };
    }
  }
}