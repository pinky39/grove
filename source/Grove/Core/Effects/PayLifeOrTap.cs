namespace Grove.Core.Effects
{
  using System;
  using System.Linq;
  using Grove.Core.Decisions;
  using Grove.Core.Decisions.Results;
  using Grove.Core.Mana;

  public class PayLifeOrTap : Effect, IProcessDecisionResults<BooleanResult>
  {
    public int Life { get; set; }

    public void ResultProcessed(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      Source.OwningCard.Tap();
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<PayOr>(Controller, p =>
        {
          p.Life = Life;
          p.Text = FormatText(String.Format("Pay {0} life?", Life));
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