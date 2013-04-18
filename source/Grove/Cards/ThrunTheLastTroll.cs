namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class ThrunTheLastTroll : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Thrun, the Last Troll")
        .ManaCost("{2}{G}{G}")
        .Type("Legendary Creature - Troll Shaman")
        .Text(
          "Thrun can't be countered.{EOL}Thrun can't be the target of spells or abilities your opponents control.{EOL}{1}{G}: Regenerate Thrun.")
        .FlavorText("His crime was silence, and now he suffers it eternally.")
        .Power(4)
        .Toughness(4)
        .Cast(p => p.Effect = () => new PutIntoPlay {CanBeCountered = false})
        .StaticAbilities(Static.Hexproof)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{G}: Regenerate Thrun.";
            p.Cost = new PayMana("{1}{G}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new Regenerate();
            p.TimingRule(new Core.Ai.TimingRules.Regenerate());
          });
    }
  }
}