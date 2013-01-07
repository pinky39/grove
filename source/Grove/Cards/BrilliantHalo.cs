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

  public class BrilliantHalo : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Brilliant Halo")
        .ManaCost("{1}{W}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature{EOL}Enchanted creature gets +1/+2.{EOL}When Brilliant Halo is put into a graveyard from the battlefield, return Brilliant Halo to its owner's hand.")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Attach>(e => e.Modifiers(Modifier<AddPowerAndToughness>(m =>
              {
                m.Power = 1;
                m.Toughness = 2;
              })));
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.CombatEnchantment();
          })               
        .Abilities(
          TriggeredAbility(
            "When Brilliant Halo is put into a graveyard from the battlefield, return Brilliant Halo to its owner's hand.",
            Trigger<OnZoneChange>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<PutToHand>(e => e.ReturnOwner = true)));
    }
  }
}