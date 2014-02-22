namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class SlowMotion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Slow Motion")
        .ManaCost("{2}{U}")
        .Type("Enchantment Aura")
        .Text(
          "At the beginning of the upkeep of enchanted creature's controller, that player sacrifices that creature unless he or she pays {2}.{EOL}When Slow Motion is put into a graveyard from the battlefield, return Slow Motion to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var tp = new TriggeredAbilityParameters
                  {
                    Text =
                      "At the beginning of the upkeep of enchanted creature's controller, that player sacrifices that creature unless he or she pays {2}.",
                    Effect =
                      () => new PayManaOrSacrifice(2.Colorless(), "Pay upkeep cost?"),
                  };

                tp.Trigger(new OnStepStart(Step.Upkeep));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.TimingRule(new OnSecondMain());
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectRankBy(c => -c.Score, ControlledBy.Opponent));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Slow Motion is put into a graveyard from the battlefield, return Slow Motion to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}