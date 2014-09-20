namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class ElvishPiper : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elvish Piper")
        .ManaCost("{3}{G}")
        .Type("Creature Elf")
        .Text("{G},{T}: You may put a creature card from your hand onto the battlefield.")
        .FlavorText(
          "From Gaea grew the world, and the world was silent. From Gaea grew the world's elves, and the world was silent no more.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{G},{T}: You may put a creature card from your hand onto the battlefield.";
            p.Cost = new AggregateCost(
              new PayMana(Mana.Green, ManaUsage.Abilities),
              new Tap());
            p.Effect = () => new PutSelectedCardToBattlefield(
              text: "Select a creature in your hand.",
              zone: Zone.Hand,
              validator: card => card.Is().Creature);
            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnFirstMain()));
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}