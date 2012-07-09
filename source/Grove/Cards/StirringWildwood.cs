namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

  public class StirringWildwood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Stirring Wildwood")
        .Type("Land")
        .Text(
          "Stirring Wildwood enters the battlefield tapped.{EOL}{T}: Add {G} or {W} to your mana pool.{EOL}{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.")
        .Timing(Timings.Lands())
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = delegate { return true; })
        .Abilities(
          C.ManaAbility(
            new Mana(ManaColors.Green | ManaColors.White),
            "{T}: Add {G} or {W} to your mana pool."),
          C.ActivatedAbility(
            "{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.",
            C.Cost<TapOwnerPayMana>((cost, _) =>
              {
                cost.Amount = "{1}{G}{W}".ParseManaAmount();
                cost.TryNotToConsumeCardsManaSourceWhenPayingThis = true;
              }),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<ChangeToCreature>((m, _) =>
                {
                  m.Power = 3;
                  m.Tougness = 4;
                  m.Colors = ManaColors.Green | ManaColors.White;
                  m.Type = "Land Creature - Elemental";
                }, untilEndOfTurn: true),
              c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Reach, untilEndOfTurn: true))),
            timing: Timings.ChangeToCreature(minAvailableMana: 4)));
    }
  }
}