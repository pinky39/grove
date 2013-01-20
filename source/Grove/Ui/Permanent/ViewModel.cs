namespace Grove.Ui.Permanent
{
  using System;
  using System.Linq;
  using Core;
  using Core.Messages;
  using Core.Targeting;
  using Infrastructure;
  using Shell;

  public class ViewModel : CardViewModel, IReceive<UiInteractionChanged>, IReceive<PlayersInterestChanged>, IReceive<AttackerSelected>,
    IReceive<AttackerUnselected>, IReceive<BlockerSelected>, IReceive<BlockerUnselected>, IReceive<RemovedFromCombat>,
    IReceive<AttackerJoinedCombat>, IReceive<BlockerJoinedCombat>, IReceive<TargetSelected>, IReceive<TargetUnselected>
  {
    private readonly CombatMarkers _combatMarkers;
    private readonly Game _game;
    private readonly SelectAbility.ViewModel.IFactory _selectAbilityVmFactory;
    private readonly SelectTarget.ViewModel.IFactory _selectTargetVmFactory;
    private readonly SelectXCost.ViewModel.IFactory _selectXCostVmFactory;
    private readonly IShell _shell;

    private Action _select = delegate { };

    public ViewModel(
      Card card,
      IShell shell,
      Game game,
      SelectAbility.ViewModel.IFactory selectAbilityVmFactory,
      SelectXCost.ViewModel.IFactory selectXCostVmFactory,
      SelectTarget.ViewModel.IFactory selectTargetVmFactory, CombatMarkers combatMarkers) : base(card)
    {
      _shell = shell;
      _game = game;
      _selectAbilityVmFactory = selectAbilityVmFactory;
      _selectXCostVmFactory = selectXCostVmFactory;
      _selectTargetVmFactory = selectTargetVmFactory;
      _combatMarkers = combatMarkers;      
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

      Marker = _combatMarkers.GetMarker(message.Attacker);
    }

    public void Receive(BlockerSelected message)
    {
      if (message.Blocker != Card)
        return;

      Marker = _combatMarkers.GetMarker(message.Attacker);
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

            IsPlayable = Card.Controller.IsHuman ? Card.CanActivateAbilities()
              .Any(x => x.CanBeSatisfied) : false;

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
      _game.Publish(new PlayersInterestChanged
        {
          Visual = this
        });
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

      if(playableActivator == null)
        return;
      
      var proceed =        
          SelectX(playableActivator, activationParameters) &&
            SelectTargets(playableActivator, activationParameters);

      if (!proceed)
        return;

      var ability = playableActivator.GetPlayable(activationParameters);

      _game.Publish(new PlayableSelected
        {
          Playable = ability
        });
    }

    private PlayableActivator SelectAbility()
    {
      var playableActivators = Card.CanActivateAbilities()
        .Select((x, i) => new PlayableActivator
          {
            Prerequisites = x,
            GetPlayable = p => new Core.Decisions.Results.PlayableAbility(Card, p, i)
          })
        .Where(x => x.Prerequisites.CanBeSatisfied)
        .ToList();

      if (playableActivators.Count == 1)
        return playableActivators[0];
      
      var dialog = _selectAbilityVmFactory.Create(playableActivators.Select(x => x.Prerequisites.Description));
      _shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

      if (dialog.WasCanceled)
        return null;

      return playableActivators[dialog.SelectedIndex];
    }

    private SelectTarget.ViewModel ShowSelectorDialog(TargetValidator validator)
    {
      var dialog = _selectTargetVmFactory.Create(validator, canCancel: true,
        instructions: "(Press Spacebar when done, press Esc to cancel.)");

      _shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      return dialog;
    }

    private bool SelectTargets(PlayableActivator activator, ActivationParameters parameters)
    {      
      if (activator.Prerequisites.TargetSelector.RequiresCostTargets)
      {
        var dialog = ShowSelectorDialog(activator.Prerequisites.TargetSelector.Cost.FirstOrDefault());

        if (dialog.WasCanceled)
          return false;

        foreach (var target in dialog.Selection)
        {
          parameters.Targets.AddCost(target);
        }
      }

      if (activator.Prerequisites.TargetSelector.RequiresEffectTargets)
      {
        foreach (var selector in activator.Prerequisites.TargetSelector.Effect)
        {
          var dialog = ShowSelectorDialog(selector);

          if (dialog.WasCanceled)
            return false;

          foreach (var target in dialog.Selection)
          {
            parameters.Targets.AddEffect(target);
          }
        }
      }

      return true;
    }

    private bool SelectX(PlayableActivator playableActivator, ActivationParameters activationParameters)
    {      
      if (playableActivator.Prerequisites.HasXInCost)
      {
        var dialog = _selectXCostVmFactory.Create(playableActivator.Prerequisites.MaxX.GetValueOrDefault());
        _shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.Disabled);

        if (dialog.WasCanceled)
          return false;

        activationParameters.X = dialog.ChosenX;
      }

      return true;
    }

    private void ChangeSelection()
    {
      _game.Publish(
        new SelectionChanged {Selection = Card});
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}