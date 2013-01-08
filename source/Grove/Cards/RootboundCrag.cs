namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Infrastructure;

  public class RootboundCrag : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Rootbound Crag")
        .Type("Land")
        .Text(
          "Rootbound Crag enters the battlefield tapped unless you control a Mountain or a Forest.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Cast(p => p.Effect = Effect<PutIntoPlay>(
          e => e.PutIntoPlayTapped = e.Controller.Battlefield.None(card => card.Is("forest") || card.Is("mountain"))))
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Red | ManaColors.Green),
            "{T}: Add {R} or {G} to your mana pool."
            ));
    }
  }
}