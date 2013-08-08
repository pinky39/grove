namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Infrastructure;

  public class RootboundCrag : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rootbound Crag")
        .Type("Land")
        .Text(
          "Rootbound Crag enters the battlefield tapped unless you control a Mountain or a Forest.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Cast(p => p.Effect = () => new PutIntoPlay(
          tap: P(e => e.Controller.Battlefield.None(card => card.Is("forest") || card.Is("mountain")))))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} or {G} to your mana pool.";
            p.ManaAmount(Mana.Colored(isRed: true, isGreen: true));
          });
    }
  }
}