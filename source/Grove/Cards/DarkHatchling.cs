namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

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
            !card.HasColor(CardColor.Black) &&
              !card.HasProtectionFrom(CardColor.Black), minCount: 1))
        )
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Dark Hatchling enters the battlefield, destroy target nonblack creature. It can't be regenerated.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black))
              .On.Battlefield());

            p.TargetingRule(new Destroy());
          });
    }
  }
}