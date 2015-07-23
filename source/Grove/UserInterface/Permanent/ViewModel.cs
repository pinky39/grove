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
      if (Marker > 0)
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
            IsPlayable = Card.Controller.IsHuman && Card.CanActivateAbilities().Count > 0;
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

      var wasCanceled =
        
        UiHelpers.SelectX(
          playableActivator.Prerequisites, 
          activationParameters, 
          canCancel: true) &&

        UiHelpers.SelectTargets(
          playableActivator.Prerequisites, 
          activationParameters, 
          canCancel: true);

      if (!wasCanceled)
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