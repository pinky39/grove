namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class CreepingTarPit : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Creeping Tar Pit")
        .Type("Land")
        .Text(
          "Creeping Tar Pit enters the battlefield tapped.{EOL}{T}: Add {U} or {B} to your mana pool.{EOL}{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(putIntoPlayTapped: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} or {B} to your mana pool.";
            p.ManaAmount(new ManaUnit(ManaColors.Blue | ManaColors.Black));
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{U}{B}: Until end of turn, Creeping Tar Pit becomes a 3/2 blue and black Elemental creature and is unblockable. It's still a land.";
            
            p.Cost = new PayMana("{1}{U}{B}".ParseMana(), ManaUsage.Abilities,
              tryNotToConsumeCardsManaSourceWhenPayingThis: true);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 2,
                type: "Land Creature Elemental",
                colors: ManaColors.Blue | ManaColors.Black) {UntilEot = true},
              () => new AddStaticAbility(Static.Unblockable) {UntilEot = true});
            
            p.TimingRule(new Core.Ai.TimingRules.ChangeToCreature(minAvailableMana: 4));
          });
    }
  }
}