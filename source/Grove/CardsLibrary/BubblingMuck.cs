namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;

  public class BubblingMuck : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bubbling Muck")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text(
          "Until end of turn, whenever a player taps a Swamp for mana, that player adds {B} to his or her mana pool (in addition to the mana the land produces).")
        .FlavorText("The muck claims a hundred living things for each meager treasure it spews forth.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPlayer(
              e => e.Controller,
              () =>
                {
                  var cp = new ContinuousEffectParameters
                    {
                      Modifier = () => new IncreaseManaOutput(Mana.Black),
                      CardFilter = (card, _) => card.Is("swamp")
                    };

                  return new AddContiniousEffect(new ContinuousEffect(cp)) {UntilEot = true};
                });
          });
    }
  }
}