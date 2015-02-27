namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class PutSelectedAttackersOnTopRestOnBottom : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates.OrderBy(x => x.Score).ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      var cardsToBottom = Game.Players.Active.Battlefield.Attackers.Except(results);

      foreach (var card in cardsToBottom)
      {
        Game.Players.Active.PutOnBottomOfLibrary(card);
      }

      foreach (var chosenCard in results)
      {
        chosenCard.PutOnTopOfLibrary();
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Game.Players.Active, p =>
        {
          p.SetValidator(c => c.IsAttacker);
          p.Zone = Zone.Battlefield;
          p.MinCount = 0;
          p.Text = "Choose attackers to put on the top of library";
          p.Instructions = "(others will be put on the bottom).";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }));
    }
  }
}