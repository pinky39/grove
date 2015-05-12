namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ConstrictingSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Constricting Sliver")
        .ManaCost("{5}{W}")
        .Type("Creature — Sliver")
        .Text(
          "Sliver creatures you control have \"When this creature enters the battlefield, you may exile target creature an opponent controls until this creature leaves the battlefield.\"")
        .FlavorText(
          "Slivers are often seen toying with enemies they capture, not out of cruelty, but to fully learn their physical capabilities.")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
        {
          p.Text =
            "When this creature enters the battlefield, you may exile target creature an opponent controls until this creature leaves the battlefield.";
          p.Effect = () => new ExileTargetsUntilOwnerLeavesBattlefield();

          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.TargetSelector.AddEffect(trg => trg
            .Is.Card(c => c.Is().Creature, ControlledBy.Opponent)
            .On.Battlefield());

          p.TargetingRule(new EffectExileBattlefield());          
        })
        .ContinuousEffect(p =>
          {
            p.CardFilter = (c, e) => c.Controller == e.Source.Controller && c.Is("sliver");

            p.Modifiers.Add(() =>
              {
                var tp = new TriggeredAbility.Parameters
                  {
                    Text =
                      "When this creature enters the battlefield, you may exile target creature an opponent controls until this creature leaves the battlefield.",
                    Effect = () => new ExileTargetsUntilOwnerLeavesBattlefield()
                  };

                tp.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                tp.TargetSelector.AddEffect(trg => trg
                  .Is.Card(c => c.Is().Creature, ControlledBy.Opponent)
                  .On.Battlefield());

                tp.TargetingRule(new EffectExileBattlefield());

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.ApplyOnlyToPermaments = false;            
          });
    }
  }
}