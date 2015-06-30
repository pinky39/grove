namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class YavimayaCoast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yavimaya Coast")
        .Type("Land")
        .Text(
          "{T}: Add {1} to your mana pool.{EOL}{T}: Add {G} or {U} to your mana pool. Yavimaya Coast deals 1 damage to you.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} or {U} to your mana pool. Yavimaya Coast deals 1 damage to you.";
            p.ManaAmount(Mana.Colored(isGreen: true, isBlue: true));
            p.AdditionalEffects.Add(() => new DealDamageToPlayer(1, P(e => e.Controller)));
            p.Priority = ManaSourcePriorities.Restricted;
          });
    }
  }
}