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
    public bool TapLandsEmptyPool;    

    protected override void ResolveEffect()
    {
      var targetSpellController = Target().Effect().Controller;

      if (DoNotCounterCost != null && targetSpellController.HasMana(DoNotCounterCost, ManaUsage.Any))
      {
        Decisions.Enqueue<AdhocDecision<BooleanResult>>(
          controller: targetSpellController,
          init: p =>
            {
              p.Param("spell", Target());
              p.QueryAi = self => { return true; };
              p.QueryUi = self =>
                {
                  var result = self.Shell.ShowMessageBox(
                    message: FormatDialogMessage(string.Format("Pay {0}?", DoNotCounterCost)),
                    buttons: MessageBoxButton.YesNo,
                    type: DialogType.Small);

                  return result == MessageBoxResult.Yes;
                };

              p.Process = self =>
                {
                  if (self.Result.IsTrue)
                  {
                    self.Controller.Consume(DoNotCounterCost, ManaUsage.Any);
                    return;
                  }

                  Counter(
                    targetSpellController: self.Controller,
                    spell: self.Param<Effect>("spell"),
                    stack: self.Game.Stack);
                };
            }
          );
        return;
      }

      Counter(targetSpellController, Target().Effect(), Game.Stack);
    }

    private void Counter(Player targetSpellController, Effect spell, Stack stack)
    {
      if (ControllersLifeloss.HasValue)
      {
        targetSpellController.Life -= ControllersLifeloss.Value;
      }

      if (TapLandsEmptyPool)
      {
        foreach (var land in targetSpellController.Battlefield.Lands)
        {
          land.Tap();
        }

        targetSpellController.EmptyManaPool();
      }

      stack.Counter(spell);
    }
  }
}