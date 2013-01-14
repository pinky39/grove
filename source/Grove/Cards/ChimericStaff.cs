namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;

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
        .Abilities(
          ActivatedAbility(
            "{X}: Chimeric Staff becomes an X/X Construct artifact creature until end of turn.",
            Cost<PayMana>(cost =>
              {
                cost.Amount = ManaAmount.Zero;
                cost.XCalculator = ChooseXAi.ChangeToXXCreature();
              }),
            Effect<Core.Effects.ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = Value.PlusX;
                  m.Toughness = Value.PlusX;
                  m.Type = "Creature Artifact Construct";
                }, untilEndOfTurn: true)
              )),
            timing: Timings.ChangeToCreature(minAvailableMana: 3)
            )
        );
    }
  }
}