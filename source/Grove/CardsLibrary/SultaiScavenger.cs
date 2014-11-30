namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SultaiScavenger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Sultai Scavenger")
          .ManaCost("{5}{B}")
          .Type("Creature — Bird Warrior")
          .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}{Flying}")
          .FlavorText("Since they guard armies of walking carrion, Sultai aven are never far from a meal.")
          .Power(3)
          .Toughness(3)
          .SimpleAbilities(Static.Flying, Static.Delve);
    }
  }
}
