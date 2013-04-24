﻿namespace Grove.Ui.Turn
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Messages;
  using Gameplay.States;
  using Infrastructure;

  public class ViewModel : IReceive<TurnStarted>
  {
    private readonly List<Ui.Step.ViewModel> _steps = new List<Ui.Step.ViewModel>();

    public ViewModel(Ui.Step.ViewModel.IFactory stepViewModelFactory)
    {
      _steps.AddRange(CreateStepViewModels(stepViewModelFactory));
      TurnNumber = 1;
    }

    public IEnumerable<Ui.Step.ViewModel> Steps { get { return _steps; } }

    public virtual int TurnNumber { get; protected set; }

    public void Receive(TurnStarted message)
    {
      TurnNumber = (message.TurnCount/2) + 1;
    }

    private static IEnumerable<Ui.Step.ViewModel> CreateStepViewModels(Ui.Step.ViewModel.IFactory factory)
    {
      yield return factory.Create("Untap", Step.Untap);
      yield return factory.Create("Upkeep", Step.Upkeep);
      yield return factory.Create("Draw", Step.Draw);
      yield return factory.Create("First main", Step.FirstMain);
      yield return factory.Create("Beg. of combat", Step.BeginningOfCombat);
      yield return factory.Create("Dec. attackers", Step.DeclareAttackers);
      yield return factory.Create("Dec. blockers", Step.DeclareBlockers);
      yield return factory.Create("Combat damage", Step.CombatDamage, Step.FirstStrikeCombatDamage);
      yield return factory.Create("End of combat", Step.EndOfCombat);
      yield return factory.Create("Second main", Step.SecondMain);
      yield return factory.Create("End of turn", Step.EndOfTurn);
      yield return factory.Create("Clean up", Step.CleanUp);
    }
  }
}