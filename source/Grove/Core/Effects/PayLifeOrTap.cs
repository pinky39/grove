namespace Grove.Core.Effects
{
  using System;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Mana;

  public class PayLifeOrTap : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly int _life;

    private PayLifeOrTap() {}

    public PayLifeOrTap(int life)
    {
      _life = life;
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      Source.OwningCard.Tap();
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<PayOr>(Controller, p =>
        {
          p.Life = _life;
          p.Text = FormatText(String.Format("Pay {0} life?", _life));
          p.Ai = decision =>
            {
              var controller = decision.Controller;

              var spellsWithCost = controller.Hand
                .Where(x => x.ManaCost != null)
                .ToList();

              if (spellsWithCost.Count == 0)
              {
                return false;
              }

              // one less is available because the land
              // is already counted
              var available = controller.GetConvertedMana(ManaUsage.Any) - 1;

              return spellsWithCost.Any(x =>
                x.ManaCost.Converted == available + 1);
            };
        });
    }
  }
}