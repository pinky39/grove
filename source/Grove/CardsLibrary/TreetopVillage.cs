namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class TreetopVillage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treetop Village")
        .Type("Land")
        .Text(
          "Treetop Village enters the battlefield tapped.{EOL}{T}: Add {G} to your mana pool.{EOL}{1}{G}: Treetop Village becomes a 3/3 green Ape creature with trample until end of turn. It's still a land.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
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

            p.Cost = new PayMana("{1}{G}".Parse());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 3,
                colors: L(CardColor.Green),
                type: t => t.Add(baseTypes: "creature", subTypes: "ape")) {UntilEot = true},
              () => new AddSimpleAbility(Static.Trample) {UntilEot = true});

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Creature));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}