namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class CarnivorousMossBeast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Carnivorous Moss-Beast")
        .ManaCost("{4}{G}{G}")
        .Type("Creature - Plant Elemental Beast")
        .Text("{5}{G}{G}: Put a +1/+1 counter on Carnivorous Moss-Beast.")
        .FlavorText("Ranger wisdom dictates that when fleeing from a moss-beast, you must stay calm, find your bearings, and run south.")
        .Power(4)
        .Toughness(5)
        .ActivatedAbility(p =>
        {
          p.Text = "Put a +1/+1 counter on Carnivorous Moss-Beast.";
          p.Cost = new PayMana("{5}{G}{G}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);

          p.Effect = () => new ApplyModifiersToSelf(
            () => new AddCounters(() => new PowerToughness(1, 1), count: 1))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TimingRule(new Any(new PumpOwningCardTimingRule(1, 1), new OnEndOfOpponentsTurn()));                    
          p.RepetitionRule(new RepeatMaxTimes());
        });
    }
  }
}
