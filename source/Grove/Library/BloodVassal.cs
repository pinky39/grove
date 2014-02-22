namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;

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
        .ManaAbility(p =>
          {
            p.Text = "Sacrifice Blood Vassal: Add {B}{B} to your mana pool.";
            p.Cost = new Sacrifice();
            p.ManaAmount("{B}{B}".Parse());
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
            p.TapRestriction = false;
          });
    }
  }
}