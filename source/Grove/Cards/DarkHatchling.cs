namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;
  using Core.Zones;

  public class DarkHatchling : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Dark Hatchling")
        .ManaCost("{4}{B}{B}")
        .Type("Creature Horror")
        .Text(
          "{Flying}{EOL}When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.")
        .Power(3)
        .Toughness(3)
        .Cast(p => p.TimingRule(new OpponentHasPermanents(
          card => card.Is().Creature &&
            !card.HasColors(ManaColors.Black) &&
              !card.HasProtectionFrom(ManaColors.Black), minCount: 1))
        )
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.";
            
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);
            
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColors(ManaColors.Black))
              .On.Battlefield());
            
            p.TargetingRule(new Destroy());
          });
    }
  }
}