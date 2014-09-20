namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class AuraThief : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aura Thief")
        .ManaCost("{3}{U}")
        .Type("Creature Illusion")
        .Text("{Flying}{EOL}When Aura Thief dies, you gain control of all enchantments. (You don't get to move Auras.)")
        .FlavorText("Illusion steals reality from the unwise.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "When Aura Thief dies, you gain control of all enchantments.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new GainControlOfAllPermanents((c, e) => c.Is().Enchantment);
          });
    }
  }
}