namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class ChimericStaff : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Chimeric Staff")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{X}: Chimeric Staff becomes an X/X Construct artifact creature until end of turn.")
        .FlavorText("A snake in the grasp.")
        .Timing(Timings.Creatures())
        .Abilities(
          ActivatedAbility(
            "{X}: Chimeric Staff becomes an X/X Construct artifact creature until end of turn.",
            Cost<TapOwnerPayMana>(cost =>
              {
                cost.Amount = ManaAmount.Zero;
                cost.HasX = true;
                cost.XCalculator = VariableCost.ChangeToXXCreature();
              }),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = Value.PlusX;
                  m.Toughness = Value.PlusX;
                  m.Type = "Creature Artifact Construct";
                  m.Colors = ManaColors.Colorless;
                }, untilEndOfTurn: true)
              )),               
            timing: Timings.ChangeToCreature(minAvailableMana: 3)
            )
        );
    }
  }
}