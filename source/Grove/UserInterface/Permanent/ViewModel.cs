namespace Grove.UserInterface.Permanent
{
  using System;
  using System.Linq;
  using Decisions;
  using Events;
  using Infrastructure;
  using Messages;
  using SelectTarget;

  public class ViewModel : CardViewModel, IReceive<UiInteractionChanged>, IReceive<PlayersInterestChanged>,
    IReceive<AttackerSelected>, IReceive<AttackerUnselected>, IReceive<BlockerSelected>, IReceive<BlockerUnselected>,
    IReceive<RemovedFromCombatEvent>, IReceive<AttackerJoinedCombatEvent>, IReceive<BlockerJoinedCombatEvent>, IReceive<TargetSelected>,
    IReceive<TargetUnselected>
  {     

    private Action _select = delegate { };

    public ViewModel(Card card) : base(card)
    {      
      RemoveAnimation = Animation.Create();
    }

    public virtual bool IsPlayable { get; protected set; }
    public virtual bool IsTargetOfSpell { get; protected set; }
    public virtual int Marker { get; protected set; }
    public virtual bool IsSelected { get; protected set; }
    public Animation RemoveAnimation { get; private set; }

    public void Receive(AttackerJoinedCombatEvent message)
    {
      if (message.Attacker.Card != Card)
        return;

      // if attacker was declared via UI do not generate marker again
      if (Card.Controller.IsHuman && message.WasDeclared)
        return;

      Marker = message.Attacker.Card.Id;
    }    

    public void Receive(AttackerSelected message)
    {
      if (message.Attacker != Card)
        return;

      Marker = message.Attacker.Id;
    }

    public void Receive(AttackerUnselected message)
    {
      if (message.Attacker != Card)
        return;
      
      Marker = 0;
    }

    public void Receive(BlockerJoinedCombatEvent message)
    {
      if (message.Blocker.Card != Card)
        return;

      if (Card.Controller.IsHuman)
        return;

      Marker = message.Attacker.Card.Id;
    }

    public void Receive(BlockerSelected message)
    {
      if (message.Blocker != Card)
        return;

      Marker = message.Attacker.Id;
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

    public void Receive(RemovedFromCombatEvent message)
    {
      if (message.Card != Card)
        return;
      
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

    public override void Initialize()
    {
      base.Initialize();
      InitializeMarker();
    }

    private void InitializeMarker()
    {
      if (Combat.IsAttacker(Card))
      {
        Marker = Card.Id;
      }
      else if (Combat.IsBlocker(Card))
      {
        var attacker = Combat.GetAttacker(Card);

        // attacker could be killed e.g when blocker has first
        // strike and this is a loaded game.
        if (attacker != null)
        {
          Marker = attacker.Id;
        }
      }
    }

    public void ChangePlayersInterest()
    {
      ChangePlayersInterest(this);
    }

    public void OnPermanentLeftBattlefield()
    {      
      RemoveAnimation.Start();
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

      Publisher.Publish(new PlayableSelected
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
      Publisher.Publish(new SelectionChanged {Selection = Card});
    }
    
    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}