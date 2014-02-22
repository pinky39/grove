namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class LlanowarBehemoth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Llanowar Behemoth")
        .ManaCost("{3}{G}{G}")
        .Type("Creature - Elemental")
        .Text("Tap an untapped creature you control: Llanowar Behemoth gets +1/+1 until end of turn.")
        .FlavorText(
          "Most people can't build decent weapons out of stone or steel. Trust the elves to do it with only mud and vines.")
        .Power(4)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text = "Tap an untapped creature you control: Llanowar Behemoth gets +1/+1 until end of turn.";
            p.Cost = new Tap();
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) {UntilEot = true})
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);              

            p.TargetSelector.AddCost(trg => trg
              .Is.Card(c => c.Is().Creature && !c.IsTapped, ControlledBy.SpellOwner)
              .On.Battlefield());

            p.TargetingRule(new EffectRankBy(c => c.Score));
            p.TimingRule(new PumpOwningCardTimingRule(1, 1));
          });
    }
  }
}