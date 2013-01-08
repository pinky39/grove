namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class ShivanDragon : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Shivan Dragon")
        .ManaCost("{4}{R}{R}")
        .Type("Creature - Dragon")
        .Text("{Flying}{EOL}{R}: Shivan Dragon gets +1/+0 until end of turn.")
        .FlavorText("The undisputed master of the mountains of Shiv.")
        .Power(5)
        .Toughness(5)        
        .Abilities(
          Static.Flying,
          ActivatedAbility(
            "{R}: Shivan Dragon gets +1/+0 until end of turn.",
            Cost<PayMana>(c => c.Amount = ManaAmount.Red),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m => m.Power = 1, untilEndOfTurn: true))),
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 0)));
    }
  }
}