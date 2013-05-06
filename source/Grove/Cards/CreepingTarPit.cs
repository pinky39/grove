namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class CreepingTarPit : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Creeping Tar Pit")
        .Type("Land")
        .Text(
          "Creeping Tar Pit enters the battlefield tapped.{EOL}{T}: Add {U} or {B} to your mana pool.{EOL}{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} or {B} to your mana pool.";
            p.ManaAmount(Mana.Colored(isBlue: true, isBlack: true));
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.";

            p.Cost = new PayMana("{1}{U}{B}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 2,
                type: "Land Creature Elemental",
                colors: L(CardColor.Blue, CardColor.Black)) {UntilEot = true},
              () => new AddStaticAbility(Static.Unblockable) {UntilEot = true});

            p.TimingRule(new Artifical.TimingRules.ChangeToCreature(minAvailableMana: 4));
          });
    }
  }
}