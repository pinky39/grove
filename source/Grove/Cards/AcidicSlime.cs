namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
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
        .Timing(Timings.Steps(Step.FirstMain))
        .Abilities(
          StaticAbility.Deathtouch,
          C.TriggeredAbility(
            "When Acidic Slime enters the battlefield, destroy target artifact, enchantment, or land.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<DestroyTargetPermanent>(),
            C.Selector(Selectors.Permanent("artifact","enchantment", "land")),                        
            targetFilter: TargetFilters.PermanentsByDescendingScore(),
            category: EffectCategories.Destruction)
        );
    }
  }
}