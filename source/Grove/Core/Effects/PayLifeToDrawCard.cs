namespace Grove.Effects
{
  using System;
  using Decisions;

  public class PayLifeToDrawCard: Effect, IProcessDecisionResults<BooleanResult>, 
    IChooseDecisionResults<BooleanResult>
  {
    private readonly int _life;

    private PayLifeToDrawCard() {}

    public PayLifeToDrawCard(int life)
    {
      _life = life;
    }

    public BooleanResult ChooseResult()
    {
      return Controller.Hand.Count < 4;
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
      {
        Controller.DrawCard();
      }      
    }

    protected override void ResolveEffect()
    {
      Enqueue(new PayOr(Controller, p =>
        {
          p.Life = _life;
          p.Text = String.Format("Pay {0} life?", _life);
          p.ChooseDecisionResults = this;
          p.ProcessDecisionResults = this;
        }));
    }
  }
}
