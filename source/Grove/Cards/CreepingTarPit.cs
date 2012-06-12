namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

  public class CreepingTarPit : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Creeping Tar Pit")
        .Type("Land")
        .Text("Creeping Tar Pit enters the battlefield tapped.{EOL}{T}: Add {U} or {B} to your mana pool.{EOL}{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.")
        .Timing(Timings.Lands)
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = delegate { return true; })
        .Abilities(
          C.ManaAbility(
            new Mana(ManaColors.Blue | ManaColors.Black),
            "{T}: Add {U} or {B} to your mana pool."),
          C.ActivatedAbility(
            "{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = "{1}{U}{B}".ParseManaAmount()),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<ChangeToCreature>((m, c0) => {
                m.Power = 3;
                m.Tougness = 2;
                m.Colors = ManaColors.Blue | ManaColors.Black;
                m.Type = "Land Creature - Elemental";
              }, untilEndOfTurn: true),
              c.Modifier<AddStaticAbility>((m, c0) => { m.StaticAbility = StaticAbility.Unblockable; }, untilEndOfTurn: true))),
            timing: Timings.Steps(Step.BeginningOfCombat)));
    }
  }
}