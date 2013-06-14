namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class WitchEngine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Witch Engine")
        .ManaCost("{5}{B}")
        .Type("Creature Horror")
        .Text(
          "{Swampwalk}{EOL}{T}: Add {B}{B}{B}{B} to your mana pool. Target opponent gains control of Witch Engine. (Activate this ability only any time you could cast an instant.)")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Swampwalk)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Add {B}{B}{B}{B} to your mana pool. Target opponent gains control of Witch Engine. (Activate this ability only any time you could cast an instant.)";

            p.Cost = new Tap();
            
            p.Effect = () => new CompoundEffect(
              new AddManaToPool("{B}{B}{B}{B}".Parse()),
              new SwitchController());

            p.TimingRule(new ControllerNeedsAdditionalMana(4));
          });
    }
  }
}