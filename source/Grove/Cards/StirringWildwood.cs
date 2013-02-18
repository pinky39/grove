namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class StirringWildwood : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Stirring Wildwood")
        .Type("Land")
        .Text(
          "Stirring Wildwood enters the battlefield tapped.{EOL}{T}: Add {G} or {W} to your mana pool.{EOL}{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} or {W} to your mana pool.";
            p.ManaAmount(new ManaUnit(ManaColors.Green | ManaColors.White));
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.";
            p.Cost = new PayMana("{1}{G}{W}".ParseMana(), ManaUsage.Abilities,
              tryNotToConsumeCardsManaSourceWhenPayingThis: true);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 4,
                colors: ManaColors.Green | ManaColors.White,
                type: "Land Creature Elemental") {UntilEot = true},
              () => new AddStaticAbility(Static.Reach) {UntilEot = true});

            p.TimingRule(new Core.Ai.TimingRules.ChangeToCreature(4));
          });
    }
  }
}