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

  public class LightningDragon : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Lightning Dragon")
        .ManaCost("{2}{R}{R}")
        .Type("Creature - Dragon")
        .Text("{Flying};{echo} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}{R}: Lightning Dragon gets +1/+0 until end of turn.")        
        .Power(4)
        .Toughness(4)        
        .Echo("{2}{R}{R}")
        .Abilities(
          Static.Flying,
          ActivatedAbility(
            "{R}: Lightning Dragon gets +1/+0 until end of turn.",
            Cost<PayMana>(c => c.Amount = ManaAmount.Red),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m => m.Power = 1, untilEndOfTurn: true))),
            timing: Timings.IncreaseOwnersPowerAndThougness(1, 0)));
    }
  }
}