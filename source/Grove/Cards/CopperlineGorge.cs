namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class CopperlineGorge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Copperline Gorge")
        .Type("Land")
        .Text(
          "Copperline Gorge enters the battlefield tapped unless you control two or fewer other lands.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Cast(p => { p.Effect = () => new PutIntoPlay(tap: P(e => e.Controller.Battlefield.Lands.Count() > 2)); })
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} or {G} to your mana pool.";
            p.ManaAmount(Mana.Colored(isRed: true, isGreen: true));                            
          });
    }
  }
}