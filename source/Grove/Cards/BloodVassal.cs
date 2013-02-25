namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;

  public class BloodVassal : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Blood Vassal")
        .ManaCost("{2}{B}")
        .Type("Creature - Thrull")
        .Text("Sacrifice Blood Vassal: Add {B}{B} to your mana pool.")
        .FlavorText("'They are bred to suffer and born to die. Much like humans.'{EOL}—Gix, Yawgmoth praetor")
        .Power(2)
        .Toughness(2)
        .ManaAbility(p =>
          {
            p.Text = "Sacrifice Blood Vassal: Add {B}{B} to your mana pool.";
            p.Cost = new Sacrifice();            
            p.ManaAmount("{B}{B}".ParseMana());
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          });
    }
  }
}