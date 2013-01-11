namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class AcidicSlime : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Acidic Slime")
        .ManaCost("{3}{G}{G}")
        .Type("Creature Ooze")
        .Text(
          "{Deathtouch}{EOL}When Acidic Slime enters the battlefield, destroy target artifact, enchantment, or land.")
        .Power(2)
        .Toughness(2)
        .Cast(p => p.Timing = Timings.FirstMain())
        .Abilities(
          Static.Deathtouch,
          TriggeredAbility(
            "When Acidic Slime enters the battlefield, destroy target artifact, enchantment, or land.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<DestroyTargetPermanents>(),
            Target(
              Validators.Card(card => card.Is().Artifact || card.Is().Enchantment || card.Is().Land),
              Zones.Battlefield()),
            TargetingAi.OrderByScore(),
            EffectCategories.Destruction)
        );
    }
  }
}