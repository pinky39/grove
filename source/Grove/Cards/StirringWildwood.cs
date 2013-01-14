namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;

  public class StirringWildwood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Stirring Wildwood")
        .Type("Land")
        .Text(
          "Stirring Wildwood enters the battlefield tapped.{EOL}{T}: Add {G} or {W} to your mana pool.{EOL}{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.")
        .Cast(p => p.Effect = Effect<Core.Effects.PutIntoPlay>(e => e.PutIntoPlayTapped = true))                  
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Green | ManaColors.White),
            "{T}: Add {G} or {W} to your mana pool."),
          ActivatedAbility(
            "{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.",
            Cost<PayMana>(cost =>
              {
                cost.Amount = "{1}{G}{W}".ParseMana();
                cost.TryNotToConsumeCardsManaSourceWhenPayingThis = true;
              }),
            Effect<Core.Effects.ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 4;
                  m.Colors = ManaColors.Green | ManaColors.White;
                  m.Type = "Land Creature - Elemental";
                }, untilEndOfTurn: true),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Reach, untilEndOfTurn: true))),
            timing: Timings.ChangeToCreature(minAvailableMana: 4)));
    }
  }
}