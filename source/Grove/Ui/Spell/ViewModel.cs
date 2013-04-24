namespace Grove.Ui.Spell
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Card.Abilities;
  using Gameplay.Decisions.Results;
  using Gameplay.Targeting;
  using Infrastructure;
  using SelectTarget;

  public class ViewModel : CardViewModel, IReceive<UiInteractionChanged>, IReceive<TargetSelected>,
    IReceive<TargetUnselected>
  {
    private Action _select = delegate { };

    public ViewModel(Card card) : base(card)
    {      
    }

    public virtual bool IsPlayable { get; protected set; }
    public virtual bool IsSelected { get; protected set; }

    public void Receive(TargetSelected message)
    {
      if (message.Target == Card)
      {
        IsSelected = true;
      }
    }

    public void Receive(TargetUnselected message)
    {
      if (message.Target == Card)
      {
        IsSelected = false;
      }
    }

    public void Receive(UiInteractionChanged message)
    {
      switch (message.State)
      {
        case (InteractionState.PlaySpellsOrAbilities):
          _select = Activate;

          if (!Card.Controller.IsHuman)
          {
            IsPlayable = false;
            return;
          }

          IsPlayable = Card.CanCast().Count > 0 || Card.CanActivateAbilities().Count > 0;
          break;

        case (InteractionState.SelectTarget):
          _select = ChangeSelection;
          IsPlayable = false;
          break;

        default:
          _select = delegate { };
          IsPlayable = false;
          break;
      }

      IsSelected = false;
    }

    public void ChangePlayersInterest()
    {
      Publish(new PlayersInterestChanged
        {
          Visual = Card
        });
    }

    public void Select()
    {
      _select();
    }

    private PlayableActivator SelectActivation()
    {
      var activations = Card.CanCast()
        .Select(prerequisites => new PlayableActivator
          {
            Prerequisites = prerequisites,
            GetPlayable = parameters => new PlayableSpell
              {
                Card = prerequisites.Card,
                ActivationParameters = parameters,
                Index = prerequisites.Index
              }
          })
        .Concat(Card.CanActivateAbilities()
          .Select(prerequisites => new PlayableActivator
            {
              Prerequisites = prerequisites,
              GetPlayable = parameters => new PlayableAbility
                {
                  Card = prerequisites.Card,
                  ActivationParameters = parameters,
                  Index = prerequisites.Index
                }
            }))
        .ToList();

      if (activations.Count == 1)
        return activations[0];

      var dialog = SelectAbilityDialog.Create(activations.Select(x => x.Prerequisites.Description));
      Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

      if (dialog.WasCanceled)
        return null;

      return activations[dialog.SelectedIndex];
    }

    private void Activate()
    {
      if (!IsPlayable)
        return;

      var activation = SelectActivation();

      if (activation == null)
        return;

      var activationParameters = new ActivationParameters();

      var proceed = SelectX(activation.Prerequisites, activationParameters) &&
        SelectTargets(activation.Prerequisites, activationParameters);

      if (!proceed)
        return;

      var playable = activation.GetPlayable(activationParameters);

      Publish(new PlayableSelected {Playable = playable});
    }

    private bool SelectTargets(ActivationPrerequisites prerequisites, ActivationParameters parameters)
    {
      if (prerequisites.Selector.RequiresCostTargets)
      {
        var dialog = ShowSelectorDialog(
          prerequisites.Selector.Cost.FirstOrDefault(),
          parameters.X);

        if (dialog.WasCanceled)
          return false;

        foreach (var target in dialog.Selection)
        {
          parameters.Targets.AddCost(target);
        }
      }

      if (prerequisites.Selector.RequiresEffectTargets)
      {
        foreach (var selector in prerequisites.Selector.Effect)
        {
          var dialog = ShowSelectorDialog(selector, parameters.X);

          if (dialog.WasCanceled)
            return false;

          // this can occur when game is terminated
          if (dialog.Selection.Count == 0)
            return false;

          foreach (var target in dialog.Selection)
          {
            parameters.Targets.AddEffect(target);
          }
        }
      }

      if (prerequisites.DistributeAmount > 0)
      {
        parameters.Targets.Distribution = DistributeDamage(parameters.Targets.Effect, prerequisites.DistributeAmount);
      }

      return true;
    }

    public List<int> DistributeDamage(IList<ITarget> targets, int damage)
    {
      if (targets.Count == 1)
      {
        return new List<int> {damage};
      }

      var dialog = DistributeDamageDialog.Create(targets, damage);
      Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

      return dialog.Distribution;
    }

    private SelectTarget.ViewModel ShowSelectorDialog(TargetValidator validator, int? x)
    {
      var selectTargetParameters = new SelectTargetParameters
        {          
          Validator = validator,
          CanCancel = true,
          Instructions = "(Press Spacebar when done, press Esc to cancel.)",
          X = x
        };

      var dialog = SelectTargetDialog.Create(selectTargetParameters);

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      return dialog;
    }

    private bool SelectX(ActivationPrerequisites prerequisites, ActivationParameters parameters)
    {
      if (prerequisites.HasXInCost)
      {
        var dialog = SelectXCostDialog.Create(prerequisites.MaxX.Value);
        Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.Disabled);

        if (dialog.WasCanceled)
          return false;

        parameters.X = dialog.ChosenX;
      }

      return true;
    }

    private void ChangeSelection()
    {
      Publish(new SelectionChanged {Selection = Card});
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}