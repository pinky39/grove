namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.Triggers;

  public class ParasiticBond : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Parasitic Bond")
        .ManaCost("{3}{B}")
        .Type("Enchantment Aura")
        .Text(
          "At the beginning of the upkeep of enchanted creature's controller, Parasitic Bond deals 2 damage to that player.")
        .FlavorText("All bonds are parasitic. Only rulership is pure.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectRankBy(c => -c.Score, ControlledBy.Opponent));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of the upkeep of enchanted creature's controller, Parasitic Bond deals 2 damage to that player.";

            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              passiveTurn: true,
              activeTurn: true)
              {
                Condition = (t, g) => t.OwningCard.AttachedTo.Controller.IsActive
              });

            p.Effect = () => new DealDamageToPlayer(
              amount: 2,
              player: P((e, g) => g.Players.Active));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}