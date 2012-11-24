namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class Despondency : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Despondency")
        .ManaCost("{1}{B}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets -2/-0.{EOL}When Despondency is put into a graveyard from the battlefield, return Despondency to its owner's hand.")
        .Timing(Timings.FirstMain())
        .Targets(
          TargetSelectorAi.ReducePower(2), 
          TargetValidator(
            TargetIs.Card(x => x.Is().Creature),
            ZoneIs.Battlefield()))
        .Effect<Attach>(
          p => p.Effect.Modifiers(Modifier<AddPowerAndToughness>(m => m.Power = -2)))
        .Abilities(
          TriggeredAbility(
            "When Despondency is put into a graveyard from the battlefield, return Despondency to its owner's hand.",
            Trigger<OnZoneChange>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<PutToHand>(e => e.AlsoReturnOwner = true)));
    }
  }
}