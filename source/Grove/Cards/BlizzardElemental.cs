namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class BlizzardElemental : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blizzard Elemental")
        .ManaCost("{5}{U}{U}")
        .Type("Creature - Elemental")
        .Text("{Flying}{EOL}{3}{U}: Untap Blizzard Elemental.")
        .FlavorText(
          "Students who had seen Rayne argue with Urza were certain she had summoned it to make him seem warmer by comparison.")
        .Power(5)
        .Toughness(5)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{3}{U}: Untap Blizzard Elemental.";
            p.Cost = new PayMana("{3}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();
            
            p.TimingRule(new Any(new OnSecondMain(), new BeforeYouDeclareAttackers()));
            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => c.IsTapped));
          });
    }
  }
}