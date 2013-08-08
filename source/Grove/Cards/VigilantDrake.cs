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

  public class VigilantDrake : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vigilant Drake")
        .ManaCost("{4}{U}")
        .Type("Creature - Drake")
        .Text("{Flying}{EOL}{2}{U}: Untap Vigilant Drake.")
        .FlavorText(
          "Awake and awing in the blink of an eye.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{U}: Untap Vigilant Drake.";
            p.Cost = new PayMana("{2}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new SecondMain());
            p.TimingRule(new StackIsEmpty());
            p.TimingRule(new OwningCardHas(c => c.IsTapped));
          });
    }
  }
}