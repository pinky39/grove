namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class SternProctor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Stern Proctor")
        .ManaCost("{U}{U}")
        .Type("Creature Human Wizard")
        .Text(
          "When Stern Proctor enters the battlefield, return target artifact or enchantment to its owner's hand.")
        .FlavorText(
          "'I preferred the harsh tutors—they made mischief all the more fun.'{EOL}—Teferi, third-level student")
        .Power(1)
        .Toughness(2)
        .Cast(p => p.Timing = All(
          Timings.FirstMain(),
          Timings.OpponentHasPermanent(card => card.Is().Artifact || card.Is().Enchantment)))
        .Abilities(
          TriggeredAbility(
            "When Stern Proctor enters the battlefield, return target artifact or enchantment to its owner's hand.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<ReturnToHand>(),
            Target(Validators.Card(
              card => card.Is().Artifact || card.Is().Enchantment), Zones.Battlefield()),
            selectorAi: TargetingAi.Bounce(),
            abilityCategory: EffectCategories.Bounce)
        );
    }
  }
}