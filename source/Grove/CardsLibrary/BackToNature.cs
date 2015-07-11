namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class BackToNature : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Back to Nature")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text("Destroy all enchantments.")
        .FlavorText("There will always be those who presume to improve nature, or dare to befoul it. And nature will always have a response.")
        .Cast(p =>
        {
          p.Effect = () => new DestroyAllPermanents((c, ctx) => c.Is().Enchantment);
          p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Enchantment));
          p.TimingRule(new Any(
            new AfterOpponentDeclaresAttackers(), 
            new BeforeYouDeclareAttackers(),
            new OnEndOfOpponentsTurn()));
        }); 
    }
  }
}
