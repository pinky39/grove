namespace Grove.Ui.Permanent
{
  using System;
  using System.Linq;
  using Core;
  using Core.Details.Cards;
  using Core.Messages;
  using Core.Targeting;
  using Infrastructure;
  using Shell;

  public class ViewModel : IReceive<SelectionModeChanged>, IReceive<PlayersInterestChanged>, IReceive<AttackerSelected>,
    IReceive<AttackerUnselected>, IReceive<BlockerSelected>, IReceive<BlockerUnselected>, IReceive<RemovedFromCombat>,
    IReceive<AttackerJoinedCombat>, IReceive<BlockerDeclared>
  {
    private readonly CombatMarkers _combatMarkers;
    private readonly Publisher _publisher;
    private readonly SelectAbility.ViewModel.IFactory _selectAbilityVmFactory;
    private readonly SelectTarget.ViewModel.IFactory _selectTargetVmFactory;
    private readonly SelectXCost.ViewModel.IFactory _selectXCostVmFactory;
    private readonly IShell _shell;

    private Action _select = delegate { };

    public ViewModel(Card card, IShell shell, Publisher publisher,
                     SelectAbility.ViewModel.IFactory selectAbilityVmFactory,
                     SelectXCost.ViewModel.IFactory selectXCostVmFactory,
                     SelectTarget.ViewModel.IFactory selectTargetVmFactory, CombatMarkers combatMarkers)
    {
      _shell = shell;
      _publisher = publisher;
      _selectAbilityVmFactory = selectAbilityVmFactory;
      _selectXCostVmFactory = selectXCostVmFactory;
      _selectTargetVmFactory = selectTargetVmFactory;
      _combatMarkers = combatMarkers;

      Card = card;
    }

    public Card Card { get; private set; }

    public virtual bool IsPlayable { get; protected set; }
    public virtual bool IsTargetOfSpell { get; protected set; }
    public virtual int Marker { get; protected set; }

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

    public void Receive(BlockerDeclared message)
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

    public void Receive(SelectionModeChanged message)
    {
      switch (message.SelectionMode)
      {
        case (SelectionMode.Play):
          {
            _select = Activate;

            IsPlayable = Card.Controller.IsHuman ? Card.CanActivateAbilities()
              .Any(x => x.CanBeSatisfied) : false;

            break;
          }
        case (SelectionMode.SelectTarget):
          {
            _select = MarkAsTarget;
            IsPlayable = false;
            break;
          }
        case (SelectionMode.Disabled):
          {
            _select = delegate { };
            IsPlayable = false;
            break;
          }
      }
    }

    public void ChangePlayersInterest()
    {
      _publisher.Publish(new PlayersInterestChanged
        {
          Visual = Card
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

      int? x = null;
      var targets = new Targets();
      SpellPrerequisites selected = null;
      var index = 0;

      var sucess =
        SelectAbility(out selected, out index) &&
          SelectX(selected, out x) &&
            SelectTargets(selected, targets);

      if (!sucess)
        return;

      var activation = new ActivationParameters(targets, x: x);
      var ability = new Core.Controllers.Results.Ability(Card, activation, index);

      _publisher.Publish(new PlayableSelected
        {
          Playable = ability
        });
    }

    private bool SelectAbility(out SpellPrerequisites selected, out int index)
    {
      var prerequisites = Card.CanActivateAbilities().ToList();

      index = 0;
      selected = null;

      if (prerequisites.Count(p => p.CanBeSatisfied) > 1)
      {
        var dialog = _selectAbilityVmFactory.Create(prerequisites);
        _shell.ShowModalDialog(dialog, DialogType.Large, SelectionMode.Disabled);

        if (dialog.WasCanceled)
          return false;

        selected = dialog.Selected;        
      }
      else
      {        
        selected = prerequisites.FirstOrDefault(x => x.CanBeSatisfied);        
      }

      index = prerequisites.IndexOf(selected);
      return true;
    }

    private SelectTarget.ViewModel ShowSelectorDialog(TargetValidator validator)
    {
      var dialog = _selectTargetVmFactory.Create(validator, canCancel: true,
        instructions: "(Press Esc to cancel.)");

      _shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);
      return dialog;
    }

    private bool SelectTargets(SpellPrerequisites prerequisites, Targets targets)
    {
      var selectors = prerequisites.TargetSelector;

      if (selectors.HasCost)
      {
        var dialog = ShowSelectorDialog(selectors.Cost.FirstOrDefault());

        if (dialog.WasCanceled)
          return false;

        foreach (var target in dialog.Selection)
        {
          targets.AddCost(target);
        }                
      }

      if (selectors.HasEffect)
      {
        foreach (var selector in selectors.Effect)
        {
          var dialog = ShowSelectorDialog(selector);

          if (dialog.WasCanceled)
            return false;

          foreach (var target in dialog.Selection)
          {
            targets.AddEffect(target);
          }
        }
      }

      return true;
    }

    private bool SelectX(SpellPrerequisites prerequisites, out int? x)
    {
      x = null;

      if (prerequisites.HasXInCost)
      {
        var dialog = _selectXCostVmFactory.Create(prerequisites.MaxX.Value);
        _shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.Disabled);

        if (dialog.WasCanceled)
          return false;

        x = dialog.ChosenX;
      }

      return true;
    }

    private void MarkAsTarget()
    {
      _publisher.Publish(
        new TargetSelected {Target = Card});
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
    }
  }
}