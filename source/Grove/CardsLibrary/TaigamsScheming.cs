namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class TaigamsScheming : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Taigam's Scheming")
        .ManaCost("{1}{U}")
        .Type("Sorcery")
        .Text("Look at the top five cards of your library. Put any number of them into your graveyard and the rest back on top of your library in any order.")
        .FlavorText("\"The Jeskai would have me bow in restraint. So I have found a people unafraid of true power.\"")
        .Cast(p =>
        {
          p.Effect = () => new PutSelectedCardsIntoGraveyardOthersOnTop(5);
          p.TimingRule(new OnSecondMain());
        });
    }
  }
}
