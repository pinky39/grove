namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class SanguineGuard : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sanguine Guard")
        .ManaCost("{1}{B}")
        .Type("Creature Zombie Knight")
        .Text("{First strike}{EOL}{1}{B}: Regenerate Sanguine Guard.")
        .FlavorText(
          "Father of Machines Your filigree gaze carves us, and the scars dance upon our grateful flesh.")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.FirstStrike)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{B}: Regenerate Sanguine Guard.";
            p.Cost = new PayMana("{1}{B}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new Regenerate();
            p.TimingRule(new Ai.TimingRules.Regenerate());
          });
    }
  }
}