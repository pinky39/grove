namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;

  public class SteamBlast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Steam Blast")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Text(
          "Steam Blast deals 2 damage to each creature and each player.")
        .FlavorText(
          "The viashino knew of the cracked pipes but deliberately left them unmended to bolster the rig's defenses.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountPlayer: 2,
              amountCreature: 2);
          });
    }
  }
}