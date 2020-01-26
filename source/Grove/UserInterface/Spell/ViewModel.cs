namespace Grove.UserInterface.Spell
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;
  using Messages;
  using SelectTarget;

  public class ViewModel : CardViewModel, IReceive<UiInteractionChanged>, IReceive<TargetSelected>,
    IReceive<TargetUnselected>
  {
    private Action _select = delegate { };

    public ViewModel(Card card) : base(card) {}

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

          IsPlayable = Card.CanCast().Count > 0 ||
            Card.CanActivateAbilities().Count > 0;
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
      ChangePlayersInterest(Card);
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

      var dialog = ViewModels.SelectAbility.Create(activations.Select(x => x.Prerequisites.Description));
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
      SelectTargets(activation.Prerequisites, activationParameters) &&
      ManuallySelectRequiredConvokeTargets() &&
      ManuallySelectRequiredDelveTargets();
    
      if (!proceed)
        return;

      var playable = activation.GetPlayable(activationParameters);

      Publisher.Publish(new PlayableSelected {Playable = playable});
    }

    private bool ManuallySelectRequiredConvokeTargets()
    {
      if (!Card.Has().Convoke)
        return true;

      var spec = new IsValidTargetBuilder()
        .Card(c => c.CanBeTapped && c.Is().Creature && c.Controller == Card.Controller)
        .On.Battlefield();

      var tp = new TargetValidatorParameters(
          isValidTarget: spec.IsValidTarget,
          isValidZone: spec.IsValidZone)
          {
            MinCount = 0,
            MaxCount = null,
            Message = "Select creatures to tap for convoke.",
            MustBeTargetable = false
          };
          
      var validator = new TargetValidator(tp);
      validator.Initialize(Game, Card.Controller);

      var dialog = ShowSelectorDialog(validator, null);
      if (dialog.WasCanceled)
      {
        return false;
      }

      foreach (var target in dialog.Selection)
      {
        target.Card().Tap();
        var manaColor = ManaColor.FromCardColors(target.Card().Colors);

        Card.Controller.AddManaToManaPool(new SingleColorManaAmount(manaColor, 1));
      }

      return true;
    }

    private bool ManuallySelectRequiredDelveTargets()
    {
      if (!Card.Has().Delve)
        return true;

      var spec = new IsValidTargetBuilder().Is.Card().In.YourGraveyard();

      var tp =
        new TargetValidatorParameters(
          isValidTarget: spec.IsValidTarget,
          isValidZone: spec.IsValidZone)
          {
            MinCount = 0,
            MaxCount = Card.HasXInCost ? int.MaxValue : Card.GenericCost,
            Message = "Select cards to exile for delve.",
            MustBeTargetable = false
          };
        
      var validator = new TargetValidator(tp);
      validator.Initialize(Game, Card.Controller);

      var dialog = ShowSelectorDialog(validator, null);
      if (dialog.WasCanceled)
      {
        return false;
      }

      foreach (var target in dialog.Selection)
      {
        target.Card().Exile();

        Card.Controller.AddManaToManaPool(new SingleColorManaAmount(ManaColor.Colorless, 1));
      }
      
      return true;
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
        parameters.Targets.Distribution = DistributeAmount(parameters.Targets.Effect, prerequisites.DistributeAmount);
      }

      return prerequisites.Selector.ValidateTargetDependencies(
        new ValidateTargetDependenciesParam 
          {
            Cost = parameters.Targets.Cost,
            Effect = parameters.Targets.Effect
          }
        );
    }

    private List<int> DistributeAmount(IList<ITarget> targets, int amount)
    {
      if (targets.Count == 1)
      {
        return new List<int> {amount};
      }

      var dialog = ViewModels.DistributeAmount.Create(targets, amount);
      Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

      return dialog.Distribution;
    }

    private SelectTarget.ViewModel ShowSelectorDialog(TargetValidator validator, int? x)
    {
      var selectTargetParameters = new SelectTargetParameters
        {
          Validator = validator,
          CanCancel = true,
          X = x
        };

      var dialog = ViewModels.SelectTarget.Create(selectTargetParameters);

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      return dialog;
    }

    private bool SelectX(ActivationPrerequisites prerequisites, ActivationParameters parameters)
    {
      if (prerequisites.HasXInCost)
      {
        var dialog = ViewModels.SelectXCost.Create(prerequisites.MaxX.Value);
        Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.Disabled);

        if (dialog.WasCanceled)
          return false;

        parameters.X = dialog.ChosenX;
      }

      return true;
    }

    private void ChangeSelection()
    {
      Publisher.Publish(new SelectionChanged {Selection = Card});
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}
