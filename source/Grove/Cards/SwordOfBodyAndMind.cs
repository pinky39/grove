namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;

  public class SwordOfBodyAndMind : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sword of Body and Mind")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from green and from blue.{EOL}Whenever equipped creature deals combat damage to a player, you put a 2/2 green Wolf creature token onto the battlefield and that player puts the top ten cards of his or her library into his or her graveyard.{EOL}{Equip} {2}")
        .Timing(Timings.Steps(Step.FirstMain))
        .Abilities(
          C.TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, you put a 2/2 green Wolf creature token onto the battlefield and that player puts the top ten cards of his or her library into his or her graveyard.",
            C.Trigger<DealDamageToCreatureOrPlayer>((t, _) =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToAnyPlayer();
              }),
            C.Effect<CompoundEffect>((e, c) => e.ChildEffects(
              c.Effect<MillOpponent>((e1, _) => e1.Count = 10),
              c.Effect<CreateTokens>((e1, c1) => e1.Tokens(
                c1.Card
                  .Named("Wolf Token")
                  .FlavorText(
                    "No matter where we cat warriors go in the world, those stupid slobberers find us.{EOL}—Mirri of the Weatherlight")
                  .Power(2)
                  .Toughness(2)
                  .Type("Creature Token Wolf")
                  .Colors(ManaColors.Green)))))),
          C.ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = 2.AsColorlessMana()),
            C.Effect<AttachEquipment>((e, c) => e.Modifiers(
              c.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              c.Modifier<AddProtectionFromColors>((m, _) => m.Colors = ManaColors.Green | ManaColors.Blue)
              )),
            selector: C.Selector(Validator.Equipment()),
            timing: Timings.AttachEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector
            ));
    }
  }
}