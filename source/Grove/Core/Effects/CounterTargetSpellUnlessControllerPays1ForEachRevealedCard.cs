namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class CounterTargetSpellUnlessControllerPays1ForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>,
    IProcessDecisionResults<BooleanResult>
  {
    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var cardsToReveal = Controller.Opponent.GetAvailableManaCount(usage: ManaUsage.Spells) + 1;
      return candidates.OrderBy(x => x.Score).Take(cardsToReveal).ToList();
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      Stack.Counter(Target.Effect());
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Reveal();
      }

      var targetSpellController = Target.Effect().Controller;

      Enqueue(new PayOr(targetSpellController, p =>
        {
          p.ManaAmount = results.Count.Colorless();
          p.Text = string.Format("Pay {0}?", results.Count);
          p.ProcessDecisionResults = this;
        }));
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller, p =>
        {
          p.SetValidator(c => c.HasColor(CardColor.Blue));
          p.Zone = Zone.Hand;
          p.MinCount = 0;
          p.Text = "Choose any number of cards in your hand.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }));
    }
  }
}