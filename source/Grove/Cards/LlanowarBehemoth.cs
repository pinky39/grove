namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.Player;

  public class LlanowarBehemoth : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              {
                Category = EffectCategories.ToughnessIncrease
              };

            p.TargetSelector.AddCost(trg => trg
              .Is.Card(c => c.Is().Creature && !c.IsTapped, ControlledBy.SpellOwner)
              .On.Battlefield());

            p.TargetingRule(new OrderByRank(c => c.Score));
            p.TimingRule(new IncreaseOwnersPowerOrToughness(1, 1));
          });
    }
  }
}