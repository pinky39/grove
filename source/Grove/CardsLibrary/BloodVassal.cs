namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Grove.Costs;

  public class BloodVassal : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blood Vassal")
        .ManaCost("{2}{B}")
        .Type("Creature - Thrull")
        .Text("Sacrifice Blood Vassal: Add {B}{B} to your mana pool.")
        .FlavorText("They are bred to suffer and born to die. Much like humans.")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {            
            p.Text = "Sacrifice Blood Vassal: Add {B}{B} to your mana pool.";
            p.Cost = new Sacrifice();
            p.Effect = () => new AddManaToPool("{B}{B}".Parse());
            p.TimingRule(new WhenYouNeedAdditionalMana(2));
            p.UsesStack = false;
          });      
    }
  }
}