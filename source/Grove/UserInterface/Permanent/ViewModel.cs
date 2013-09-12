namespace Grove.UserInterface.Permanent
{
  using System;
  using System.Linq;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Decisions.Results;
  using Gameplay.Messages;
  using Gameplay.Targeting;
  using Infrastructure;
  using Messages;
  using SelectTarget;

  public class ViewModel : CardViewModel, IReceive<UiInteractionChanged>, IReceive<PlayersInterestChanged>,
    IReceive<AttackerSelected>, IReceive<AttackerUnselected>, IReceive<BlockerSelected>, IReceive<BlockerUnselected>,
    IReceive<RemovedFromCombat>, IReceive<AttackerJoinedCombat>, IReceive<BlockerJoinedCombat>, IReceive<TargetSelected>,
    IReceive<TargetUnselected>
  {
    private readonly CombatMarkers _combatMarkers;

    private Action _select = delegate { };

    public ViewModel(Card card, CombatMarkers combatMarkers) : base(card)
    {
      _combatMarkers = combatMarkers;      
    }

    public override void Initialize()
    {
      base.Initialize();
      InitializeMarker();
    }

    private void InitializeMarker()
    {
      if (Combat.IsAttacker(Card))
      {
        Marker = _combatMarkers.GenerateMarker(Card);
      }
      else if (Combat.IsBlocker(Card))
      {
        var attacker = Combat.GetAttacker(Card);        

        // attacker could be killed e.g when blocker has first
        // strike and this is a loaded game.
        if (attacker != null)
        {
          Marker = _combatMarkers.GenerateMarker(attacker);
        }
      }
    }

    public virtual bool IsPlayable { get; protected set; }
    public virtual bool IsTargetOfSpell { get; protected set; }
    public virtual int Marker { get; protected set; }
    public virtual bool IsSelected { get; protected set; }

    public void Receive(AttackerJoinedCombat message)
    {
      if (message.Attacker.Card != Card)
        return;

      // if attacker was declared via UI do not generate marker again
      if (Card.Controller.IsHuman && message.WasDeclared)
        return;

      Marker = _combatMarkers.GenerateMarker(message.Attacker);
    }

    public void Receive(AttackerSelected message)
    {
      if (message.Attacker != Card)
        return;

      Marker = _combatMarkers.GenerateMarker(message.Attacker);
    }

    public void Receive(AttackerUnselected message)
    {
      if (message.Attacker != Card)
        return;

      _combatMarkers.ReleaseMarker(message.Attacker);
      Marker = 0;
    }

    public void Receive(BlockerJoinedCombat message)
    {
      if (message.Blocker.Card != Card)
        return;

      if (Card.Controller.IsHuman)
        return;

      Marker = _combatMarkers.GenerateMarker(message.Attacker);
    }

    public void Receive(BlockerSelected message)
    {
      if (message.Blocker != Card)
        return;

      Marker = _combatMarkers.GenerateMarker(message.Attacker);
    }

    public void Receive(BlockerUnselected message)
    {
      if (message.Blocker != Card)
        return;

      Marker = 0;
    }

    public void Receive(PlayersInterestChanged message)
    {
      if (message.InterestedInTarget(Card) == false)
        return;

      IsTargetOfSpell = !message.HasLostInterest;
    }

    public void Receive(RemovedFromCombat message)
    {
      if (message.Card != Card)
        return;

      _combatMarkers.ReleaseMarker(message.Card);
      Marker = 0;
    }

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
          {
            _select = Activate;
            IsPlayable = Card.Controller.IsHuman ? Card.CanActivateAbilities().Count(x => x.CanPay.Value) > 0 : false;
            break;
          }
        case (InteractionState.SelectTarget):
          {
            _select = ChangeSelection;
            IsPlayable = false;
            break;
          }
        case (InteractionState.Disabled):
          {
            _select = delegate { };
            IsPlayable = false;
            break;
          }
      }

      IsSelected = false;
    }

    public void ChangePlayersInterest()
    {
      ChangePlayersInterest(this);
    }

    public virtual void Close()
    {
      _combatMarkers.ReleaseMarker(Card);
    }

    public void Select()
    {
      _select();
    }

    private void Activate()
    {
      if (!IsPlayable)
        return;

      var activationParameters = new ActivationParameters();

      var playableActivator = SelectAbility();

      if (playableActivator == null)
        return;

      var proceed =
        SelectX(playableActivator, activationParameters) &&
          SelectTargets(playableActivator, activationParameters);

      if (!proceed)
        return;

      var ability = playableActivator.GetPlayable(activationParameters);

      Shell.Publish(new PlayableSelected
        {
          Playable = ability
        });
    }

    private PlayableActivator SelectAbility()
    {
      var playableActivators = Card.CanActivateAbilities()
        .Where(x => x.CanPay.Value)
        .Select(prerequisites => new PlayableActivator
          {
            Prerequisites = prerequisites,
            GetPlayable = p => new PlayableAbility
              {
                Card = Card,
                ActivationParameters = p,
                Index = prerequisites.Index
              }
          })
        .ToList();

      if (playableActivators.Count == 1)
        return playableActivators[0];

      var dialog = ViewModels.SelectAbility.Create(playableActivators.Select(x => x.Prerequisites.Description));
      Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

      if (dialog.WasCanceled)
        return null;

      return playableActivators[dialog.SelectedIndex];
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

      var dialog = ViewModels.SelectTarget.Create(selectTargetParameters);
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      return dialog;
    }

    private bool SelectTargets(PlayableActivator activator, ActivationParameters parameters)
    {
      if (activator.Prerequisites.Selector.RequiresCostTargets)
      {
        var dialog = ShowSelectorDialog(
          activator.Prerequisites.Selector.Cost.FirstOrDefault(),
          parameters.X);

        if (dialog.WasCanceled)
          return false;

        foreach (var target in dialog.Selection)
        {
          parameters.Targets.AddCost(target);
        }
      }

      if (activator.Prerequisites.Selector.RequiresEffectTargets)
      {
        foreach (var selector in activator.Prerequisites.Selector.Effect)
        {
          var dialog = ShowSelectorDialog(selector, parameters.X);

          if (dialog.WasCanceled)
            return false;

          foreach (var target in dialog.Selection)
          {
            parameters.Targets.AddEffect(target);
          }
        }
      }

      return activator.Prerequisites.Selector.ValidateTargetDependencies(
        new ValidateTargetDependenciesParam
          {
            Cost = parameters.Targets.Cost,
            Effect = parameters.Targets.Effect
          }
        );
    }

    private bool SelectX(PlayableActivator playableActivator, ActivationParameters activationParameters)
    {
      if (playableActivator.Prerequisites.HasXInCost)
      {
        var dialog = ViewModels.SelectXCost.Create(playableActivator.Prerequisites.MaxX.Value.Value);
        Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.Disabled);

        if (dialog.WasCanceled)
          return false;

        activationParameters.X = dialog.ChosenX;
      }

      return true;
    }

    private void ChangeSelection()
    {
      Shell.Publish(new SelectionChanged {Selection = Card});
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}