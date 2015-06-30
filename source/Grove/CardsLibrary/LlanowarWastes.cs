namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class LlanowarWastes : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Llanowar Wastes")
        .Type("Land")
        .Text(
          "{T}: Add {1} to your mana pool.{EOL}{T}: Add {B} or {G} to your mana pool. Llanowar Wastes deals 1 damage to you.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {B} or {G} to your mana pool. Llanowar Wastes deals 1 damage to you.";
            p.ManaAmount(Mana.Colored(isBlack: true, isGreen: true));
            p.AdditionalEffects.Add(() => new DealDamageToPlayer(1, P(e => e.Controller)));
            p.Priority = ManaSourcePriorities.Restricted;
          });
    }
  }
}