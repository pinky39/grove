namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class CreepingTarPit : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Creeping Tar Pit")
        .Type("Land")
        .Text(
          "Creeping Tar Pit enters the battlefield tapped.{EOL}{T}: Add {U} or {B} to your mana pool.{EOL}{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.")
        .Timing(Timings.Lands())
        .Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true)
        .Abilities(
          C.ManaAbility(
            new ManaUnit(ManaColors.Blue | ManaColors.Black),
            "{T}: Add {U} or {B} to your mana pool."),
          C.ActivatedAbility(
            "{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.",
            C.Cost<TapOwnerPayMana>((cost, _) =>
              {
                cost.Amount = "{1}{U}{B}".ParseManaAmount();
                cost.TryNotToConsumeCardsManaSourceWhenPayingThis = true;
              }),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<ChangeToCreature>((m, c0) =>
                {
                  m.Power = 3;
                  m.Tougness = 2;
                  m.Colors = ManaColors.Blue | ManaColors.Black;
                  m.Type = "Land Creature - Elemental";
                }, untilEndOfTurn: true),
              p.Builder.Modifier<AddStaticAbility>((m, c0) => { m.StaticAbility = Static.Unblockable; }, untilEndOfTurn: true))),
            timing: Timings.ChangeToCreature(minAvailableMana: 4)));
    }
  }
}