namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class CrystalChimes : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Crystal Chimes")
        .ManaCost("{3}")
        .Type("Artifact")        
        .Text("{3},{T}, Sacrifice Crystal Chimes: Return all enchantment cards from your graveyard to your hand.")
        .FlavorText("As Serra was to learn, the peace and sanctity of her realm were as fragile as glass.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{3},{T}, Sacrifice Crystal Chimes: Return all enchantment cards from your graveyard to your hand.";
            p.Cost = new AggregateCost(
              new PayMana(3.Colorless(), ManaUsage.Abilities),
              new Tap(),
              new Sacrifice());
            p.Effect = () => new ReturnAllCardsInGraveyardToHand(c => c.Is().Enchantment);
            p.TimingRule(new ControllerGravayardCountIs(minCount: 2, selector: c => c.Is().Enchantment));
          }
        );
    }
  }
}