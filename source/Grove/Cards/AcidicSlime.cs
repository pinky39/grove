namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class AcidicSlime : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Acidic Slime")
        .ManaCost("{3}{G}{G}")
        .Type("Creature Ooze")
        .Text(
          "{Deathtouch}{EOL}When Acidic Slime enters the battlefield, destroy target artifact, enchantment, or land.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.FirstMain())
        .Abilities(
          Static.Deathtouch,
          C.TriggeredAbility(
            "When Acidic Slime enters the battlefield, destroy target artifact, enchantment, or land.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<DestroyTargetPermanent>(),
            C.Validator(Validators.Permanent(
              card => card.Is().Artifact || card.Is().Enchantment || card.Is().Land)),
            aiTargetSelector: AiTargetSelectors.OrderByDescendingScore(),
            abilityCategory: EffectCategories.Destruction)
        );
    }
  }
}