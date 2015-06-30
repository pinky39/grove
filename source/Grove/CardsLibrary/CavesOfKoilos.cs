namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class CavesOfKoilos : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Caves of Koilos")
        .Type("Land")
        .Text(
          "{T}: Add {1} to your mana pool.{EOL}{T}: Add {W} or {B} to your mana pool. Caves of Koilos deals 1 damage to you.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} or {B} to your mana pool. Caves of Koilos deals 1 damage to you.";
            p.ManaAmount(Mana.Colored(isBlack: true, isWhite: true));
            p.AdditionalEffects.Add(() => new DealDamageToPlayer(1, P(e => e.Controller)));
            p.Priority = ManaSourcePriorities.Restricted;
          });
    }
  }
}