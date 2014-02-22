namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class GhituEncampment : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ghitu Encampment")
        .Type("Land")
        .Text(
          "Ghitu Encampment enters the battlefield tapped.{EOL}{T}: Add {R} to your mana pool.{EOL}{1}{R}: Ghitu Encampment becomes a 2/1 red Warrior creature with first strike until end of turn. It's still a land.")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} to your mana pool.";
            p.ManaAmount(Mana.Red);
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{R}: Ghitu Encampment becomes a 2/1 red Warrior creature with first strike until end of turn. It's still a land.";

            p.Cost = new PayMana("{1}{R}".Parse(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 2,
                toughness: 1,
                colors: L(CardColor.Red),
                type: "Land Creature Warrior") { UntilEot = true },
              () => new AddStaticAbility(Static.FirstStrike) { UntilEot = true });

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Creature));
            p.TimingRule(new WhenYouHaveMana(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}