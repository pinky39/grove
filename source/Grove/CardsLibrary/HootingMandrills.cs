namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class HootingMandrills : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Hooting Mandrills")
          .ManaCost("{5}{G}")
          .Type("Creature — Ape")
          .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}{Trample}")
          .FlavorText("Interlopers in Sultai territory usually end up as crocodile chow or baboon bait.")
          .Power(4)
          .Toughness(4)
          .SimpleAbilities(Static.Trample, Static.Delve);
    }
  }
}
