namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class BloodVassal : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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