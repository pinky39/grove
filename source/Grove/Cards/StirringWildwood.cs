namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

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
            p.ManaAmount(Mana.Colored(isGreen: true, isWhite: true));
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 green and white Elemental creature with reach. It's still a land.";
            p.Cost = new PayMana("{1}{G}{W}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 3,
                toughness: 4,
                colors: L(CardColor.Green, CardColor.White),
                type: "Land Creature Elemental") {UntilEot = true},
              () => new AddStaticAbility(Static.Reach) {UntilEot = true});

            p.TimingRule(new StackIsEmpty());
            p.TimingRule(new OwningCardHas(c => !c.Is().Creature));
            p.TimingRule(new Artifical.TimingRules.ChangeToCreature(4));
          });
    }
  }
}