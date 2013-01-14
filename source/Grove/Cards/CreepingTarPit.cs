namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;

  public class CreepingTarPit : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Creeping Tar Pit")
        .Type("Land")
        .Text(
          "Creeping Tar Pit enters the battlefield tapped.{EOL}{T}: Add {U} or {B} to your mana pool.{EOL}{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.")
        .Cast(p => p.Effect = Effect<Core.Effects.PutIntoPlay>(e => e.PutIntoPlayTapped = true))
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Blue | ManaColors.Black),
            "{T}: Add {U} or {B} to your mana pool."),
          ActivatedAbility(
            "{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.",
            Cost<PayMana>(cost =>
              {
                cost.Amount = "{1}{U}{B}".ParseMana();
                cost.TryNotToConsumeCardsManaSourceWhenPayingThis = true;
              }),
            Effect<Core.Effects.ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 2;
                  m.Colors = ManaColors.Blue | ManaColors.Black;
                  m.Type = "Land Creature - Elemental";
                }, untilEndOfTurn: true),
              Modifier<AddStaticAbility>(m => { m.StaticAbility = Static.Unblockable; }, untilEndOfTurn: true))),
            timing: Timings.ChangeToCreature(minAvailableMana: 4)));
    }
  }
}