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
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 2,
                toughness: 1,
                colors: L(CardColor.Blue),
                type: "Land Creature Faerie") {UntilEot = true},
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new StackIsEmpty());
            p.TimingRule(new OwningCardHas(c => !c.Is().Creature));
            p.TimingRule(new Artifical.TimingRules.ChangeToCreature(minAvailableMana: 3));
          });
    }
  }
}