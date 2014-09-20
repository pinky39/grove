namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class FaerieConclave : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Faerie Conclave")
        .Type("Land")
        .Text(
          "Faerie Conclave enters the battlefield tapped.{EOL}{T}: Add {U} to your mana pool.{EOL}{1}{U}: Faerie Conclave becomes a 2/1 blue Faerie creature with flying until end of turn. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} to your mana pool.";
            p.ManaAmount(Mana.Blue);
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{U}: Faerie Conclave becomes a 2/1 blue Faerie creature with flying until end of turn. It's still a land.";

            p.Cost = new PayMana("{1}{U}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 2,
                toughness: 1,
                colors: L(CardColor.Blue),
                type: "Land Creature Faerie") {UntilEot = true},
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Creature));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}