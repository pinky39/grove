namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class Fortitude : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fortitude")
        .ManaCost("{1}{G}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Sacrifice a Forest: Regenerate enchanted creature.{EOL}When Fortitude is put into a graveyard from the battlefield, return Fortitude to its owner's hand.")
        .Effect<Attach>(e => e.Modifiers(
          Modifier<AddActivatedAbility>(m => m.Ability =
            ActivatedAbility(
              "Sacrifice a Forest: Regenerate enchanted creature.",
              Cost<SacPermanent>(),
              Effect<Regenerate>(),
              costValidator:
                TargetValidator(
                  TargetIs.Card(card => card.Is("forest"), Controller.SpellOwner),
                  ZoneIs.Battlefield(),
                  text: "Select a forest to sacrifice.", mustBeTargetable: false),
              targetSelectorAi: TargetSelectorAi.CostSacrificeRegenerate(),
              timing: Timings.Regenerate())
            )))
        .Timing(Timings.FirstMain())
        .Targets(
          TargetSelectorAi.CombatEnchantment(),
          TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield()))
        .Abilities(
          TriggeredAbility(
            "When Fortitude is put into a graveyard from the battlefield, return Fortitude to its owner's hand.",
            Trigger<OnZoneChange>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<PutToHand>(e => e.AlsoReturnOwner = true)));
    }
  }
}