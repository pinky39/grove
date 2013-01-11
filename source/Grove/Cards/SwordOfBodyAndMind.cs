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
  using Core.Mana;
  using Core.Targeting;

  public class SwordOfBodyAndMind : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sword of Body and Mind")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from green and from blue.{EOL}Whenever equipped creature deals combat damage to a player, you put a 2/2 green Wolf creature token onto the battlefield and that player puts the top ten cards of his or her library into his or her graveyard.{EOL}{Equip} {2}")
        .Cast(p => p.Timing = Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, you put a 2/2 green Wolf creature token onto the battlefield and that player puts the top ten cards of his or her library into his or her graveyard.",
            Trigger<OnDamageDealt>(t =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToPlayer();
              }),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<MillOpponent>(e1 => e1.Count = 10),
              Effect<CreateTokens>(e1 => e1.Tokens(
                Card
                  .Named("Wolf Token")
                  .FlavorText(
                    "No matter where we cat warriors go in the world, those stupid slobberers find us.{EOL}—Mirri of the Weatherlight")
                  .Power(2)
                  .Toughness(2)
                  .Type("Creature Token Wolf")
                  .Colors(ManaColors.Green)))))),
          ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            Cost<PayMana>(cost => cost.Amount = 2.Colorless()),
            Effect<Attach>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              Modifier<AddProtectionFromColors>(m => m.Colors = ManaColors.Green | ManaColors.Blue)
              )),
            effectTarget: Target(Validators.ValidEquipmentTarget(), Zones.Battlefield()),
            targetingAi: TargetingAi.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector
            ));
    }
  }
}