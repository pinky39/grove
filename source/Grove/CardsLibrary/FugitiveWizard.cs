namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class FugitiveWizard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fugitive Wizard")
        .ManaCost("{U}")
        .Type("Creature - Human Wizard")
        .FlavorText("\"The law has its place—as a footnote in my spellbook.\"{EOL}—Siyani, fugitive mage")
        .Power(1)
        .Toughness(1);
    }
  }
}