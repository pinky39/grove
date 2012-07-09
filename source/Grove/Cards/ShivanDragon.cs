namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

  public class ShivanDragon : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Shivan Dragon")
        .ManaCost("{4}{R}{R}")
        .Type("Creature - Dragon")
        .Text("{Flying}{EOL}{R}: Shivan Dragon gets +1/+0 until end of turn.")
        .FlavorText("The undisputed master of the mountains of Shiv.")
        .Power(5)
        .Toughness(5)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          C.ActivatedAbility(
            "{R}: Shivan Dragon gets +1/+0 until end of turn.",
            C.Cost<TapOwnerPayMana>((c, _) => c.Amount = Mana.Red.ToAmount()),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<AddPowerAndToughness>((m, _) => m.Power = 1, untilEndOfTurn: true))),
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 0)));
    }
  }
}