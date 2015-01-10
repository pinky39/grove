namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class ScoutTheBorders : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scout the Borders")
        .ManaCost("{2}{G}")
        .Type("Sorcery")
        .Text("Reveal the top five cards of your library. You may put a creature or land card from among them into your hand. Put the rest into your graveyard.")
        .FlavorText("\"I am in my element: the element of surprise.\"{EOL}—Mogai, Sultai scout")
        .Cast(p =>
        {
          p.Effect = () => new RevealTopCardsPutOneInHandOthersIntoGraveyard(5, c => c.Is().Land || c.Is().Creature);
        });
    }
  }
}
