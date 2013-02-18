namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class TrollAscetic : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Troll Ascetic")
        .ManaCost("{1}{G}{G}")
        .Type("Creature - Troll Shaman")
        .Text(
          "{Hexproof}{EOL}{1}{G}: Regenerate Troll Ascetic.")
        .FlavorText("It's no coincidence that the oldest trolls are also the angriest.")
        .Power(3)
        .Toughness(2)
        .StaticAbilities(Static.Hexproof)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{G}: Regenerate Troll Ascetic.";
            p.Cost = new PayMana("{1}{G}".ParseMana(), ManaUsage.Abilities);
            p.Effect = () => new Regenerate();
            p.TimingRule(new Core.Ai.TimingRules.Regenerate());
          });
    }
  }
}