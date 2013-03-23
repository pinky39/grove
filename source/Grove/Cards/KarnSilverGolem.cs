namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;
  using Core.Triggers;

  public class KarnSilverGolem : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Karn, Silver Golem")
        .ManaCost("{5}")
        .Type("Legendary Artifact Creature Golem")
        .Text(
          "Whenever Karn, Silver Golem blocks or becomes blocked, it gets -4/+4 until end of turn.{EOL}{1}: Target noncreature artifact becomes an artifact creature with power and toughness each equal to its converted mana cost until end of turn.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Karn, Silver Golem blocks or becomes blocked, it gets -4/+4 until end of turn.";
            p.Trigger(new OnBlock(becomesBlocked: true, blocks: true));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(-4, 4) {UntilEot = true});
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}: Target noncreature artifact becomes an artifact creature with power and toughness each equal to its converted mana cost until end of turn.";
            p.Cost = new PayMana(1.Colorless(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToTargets(() => new ChangeToCreature(
              power: m => m.Target.Card().ConvertedCost,
              toughness: m => m.Target.Card().ConvertedCost,
              type: m => m.Target.Card().Type + " Creature") {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Artifact && !c.Is().Creature)
              .On.Battlefield());

            p.TimingRule(new Core.Ai.TimingRules.ChangeToCreature());
            p.TargetingRule(new OrderByRank(c => -c.ConvertedCost, ControlledBy.SpellOwner));
          });
    }
  }
}