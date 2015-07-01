namespace Grove.Effects
{
  using System;
  using System.Linq;
  using Decisions;

  public class PayLifeOrTapLand : Effect, IProcessDecisionResults<BooleanResult>, 
    IChooseDecisionResults<BooleanResult>
  {
    private readonly int _life;

    private PayLifeOrTapLand() {}

    public PayLifeOrTapLand(int life)
    {
      _life = life;
    }

    public BooleanResult ChooseResult()
    {
      var controller = Controller;

      var spellsWithCost = controller.Hand
        .Where(x => x.ManaCost != null)
        .ToList();

      if (spellsWithCost.Count == 0)
      {
        return false;
      }

      // one less is available because the land
      // is already counted
      var available = controller.GetAvailableManaCount() - 1;

      return spellsWithCost.Any(x =>
        x.ManaCost.Converted == available + 1);
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      Source.OwningCard.Tap();
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