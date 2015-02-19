namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class ActivePlayerPaysLifeOrReturnSelectedPermanentToHand : Effect,
    IProcessDecisionResults<BooleanResult>,
    IChooseDecisionResults<BooleanResult>, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _life;

    public ActivePlayerPaysLifeOrReturnSelectedPermanentToHand(int life)
    {
      _life = life;
    }

    private ActivePlayerPaysLifeOrReturnSelectedPermanentToHand() {}

    public BooleanResult ChooseResult()
    {
      return Controller.Life > 6 && Controller.Battlefield.Count < 6;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => x.Score)
        .Take(1)
        .ToList();
    }

    public void ProcessResults(BooleanResult results)
    {
      Enqueue(new SelectCards(
        Players.Active,
        p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.SetValidator(delegate { return true; });
            p.Zone = Zone.Battlefield;
            p.Text = "Select a permanent to return to hand.";
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }));
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToHand();
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue(new PayOr(Players.Active, p =>
        {
          p.Life = _life;
          p.Text = String.Format("Pay {0} life?", _life);
          p.ChooseDecisionResults = this;
          p.ProcessDecisionResults = this;
        }));
    }
  }
}