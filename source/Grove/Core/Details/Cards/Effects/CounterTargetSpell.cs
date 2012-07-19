namespace Grove.Core.Details.Cards.Effects
{
  using System.Windows;
  using Controllers;
  using Controllers.Results;
  using Mana;
  using Targeting;
  using Ui;
  using Zones;

  public class CounterTargetSpell : Effect
  {
    public int? ControllersLifeloss;
    public IManaAmount DoNotCounterCost;

    protected override void ResolveEffect()
    {
      var targetSpellController = Target().Effect().Controller;

      if (DoNotCounterCost != null && targetSpellController.HasMana(DoNotCounterCost))
      {
        Decisions.Enqueue<AdHocDecision<BooleanResult>>(
          controller: targetSpellController,
          init: p =>
            {
              p.Param("spell", Target());
              p.QueryAi = self => { return true; };
              p.QueryUi = shell =>
                {
                  var result = shell.ShowMessageBox(
                    message: string.Format("Pay {0}?", DoNotCounterCost),
                    buttons: MessageBoxButton.YesNo,
                    type: DialogType.Small);

                  return result == MessageBoxResult.Yes;
                };

              p.Process = self =>
                {
                  if (self.Result.IsTrue)
                  {
                    self.Controller.Consume(DoNotCounterCost);
                    return;
                  }

                  Counter(
                    targetSpellController: self.Controller,
                    spell: self.Param<Effect>("spell"),
                    stack: self.Game.Stack);
                };
            }
          );
      }

      Counter(targetSpellController, Target().Effect(), Game.Stack);
    }

    private void Counter(Player targetSpellController, Effect spell, Stack stack)
    {
      if (ControllersLifeloss.HasValue)
      {
        targetSpellController.Life -= ControllersLifeloss.Value;
      }

      stack.Counter(spell);
    }
  }
}