namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class ThranLens : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Lens")
        .Type("Artifact")
        .ManaCost("{2}")
        .Text("All permanents are colorless.")
        .FlavorText(
          "Every device in the rig is evidence of Thran enlightenment. All mana was the same to them, whether from rock or water, growth or decay. Can you imagine such unity of vision?")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new SetColors(CardColor.Colorless);
            p.CardFilter = (card, source) => true;
          });
    }
  }
}