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

  public class TreetopVillage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treetop Village")
        .Type("Land")
        .Text(
          "Treetop Village enters the battlefield tapped.{EOL}{T}: Add {G} to your mana pool.{EOL}{1}{G}: Treetop Village becomes a 3/3 green Ape creature with trample until end of turn. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(Mana.Green);
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{G}: Treetop Village becomes a 3/3 green Ape creature with trample until end of turn. It's still a land.";

            p.Cost = new PayMana("{1}{G}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 3,
                toughness: 3,
                colors: L(CardColor.Green),
                type: "Land Creature Ape") {UntilEot = true},
              () => new AddStaticAbility(Static.Trample) {UntilEot = true});

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Creature));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}