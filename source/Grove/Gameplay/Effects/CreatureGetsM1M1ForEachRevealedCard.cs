namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Modifiers;

  public class CreatureGetsM1M1ForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var cardsToReveal = Target.Card().Toughness.GetValueOrDefault();
      return candidates.OrderBy(x => x.Score).Take(cardsToReveal).ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Reveal();
      }

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      var modifier = new AddPowerAndToughness(-results.Count, -results.Count);
      Target.Card().AddModifier(modifier, p);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller, p =>
        {
          p.SetValidator(c => c.HasColor(CardColor.Black));
          p.Zone = Zone.Hand;
          p.MinCount = 0;
          p.Text = "Choose any number of black cards in your hand.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }));
    }
  }
}