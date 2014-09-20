namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;

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
            
            p.TimingRule(new Any(new OnSecondMain(), new BeforeYouDeclareAttackers()));
            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => c.IsTapped));
          });
    }
  }
}