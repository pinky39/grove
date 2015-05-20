namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class JaliraMasterPolymorphist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jalira, Master Polymorphist")
        .ManaCost("{3}{U}")
        .Type("Legendary Creature — Human Wizard")
        .Text(
          "{2}{U}, {T}, Sacrifice another creature: Reveal cards from the top of your library until you reveal a nonlegendary creature card. Put that card onto the battlefield and the rest on the bottom of your library in a random order.")
        .FlavorText("\"You can become anything if I just put my mind to it.\"")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{U}, {T}, Sacrifice another creature: Reveal cards from the top of your library until you reveal a nonlegendary creature card. Put that card onto the battlefield and the rest on the bottom of your library in a random order.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{U}".Parse()),
              new TapOwner(),
              new Sacrifice());

            p.TargetSelector.AddCost(
              trg => trg
                .Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false)
                .On.Battlefield(),
              trg => trg.Message = "Select a creature to sacrifice.");

            p.Effect = () => new PutFirstCardInPlayPutOtherCardsToZone(
              toZone: Zone.Library,
              filter: c => c.Is().Creature && !c.Is().Legendary);

            p.TargetingRule(new EffectOrCostRankBy(c => c.Score));
          });
    }
  }
}