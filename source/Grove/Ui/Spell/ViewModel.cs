namespace Grove.Ui.Spell
{
  using System;
  using System.Windows;
  using Core;
  using Core.Controllers.Results;
  using Core.Details.Cards;
  using Core.Targeting;
  using Infrastructure;
  using Shell;

  public class ViewModel : IReceive<SelectionModeChanged>
  {
    private readonly Publisher _publisher;
    private readonly SelectAbility.ViewModel.IFactory _selectAbilityVmFactory;
    private readonly SelectTarget.ViewModel.IFactory _selectTargetVmFactory;
    private readonly SelectXCost.ViewModel.IFactory _selectXCostVmFactory;
    private readonly IShell _shell;
    private Action _select;

    public ViewModel(
      Card card,
      IShell shell,
      Publisher publisher,
      SelectTarget.ViewModel.IFactory selectTargetVmFactory,
      SelectXCost.ViewModel.IFactory selectXCostVmFactory,
      SelectAbility.ViewModel.IFactory selectAbilityVmFactory)
    {
      _shell = shell;
      _publisher = publisher;
      _selectTargetVmFactory = selectTargetVmFactory;
      _selectXCostVmFactory = selectXCostVmFactory;
      _selectAbilityVmFactory = selectAbilityVmFactory;
      _select = delegate { };
      Card = card;
    }

    public Card Card { get; private set; }
    public virtual bool IsPlayable { get; protected set; }

    public void Receive(SelectionModeChanged message)
    {
      switch (message.SelectionMode)
      {
        case (SelectionMode.Play):
          _select = Activate;
          IsPlayable = Card.Controller.IsHuman
            ? Card.CanCast().CanBeSatisfied || Card.CanCycle().CanBeSatisfied : false;
          break;

        case (SelectionMode.SelectTarget):
          _select = MarkAsTarget;
          IsPlayable = false;
          break;

        default:
          _select = delegate { };
          IsPlayable = false;
          break;
      }
    }

    public void ChangePlayersInterest()
    {
      _publisher.Publish(new PlayersInterestChanged
        {
          Visual = Card
        });
    }

    public void Select()
    {
      _select();
    }

    private bool SelectActivation(out bool isCast, out SpellPrerequisites prerequisites)
    {
      isCast = false;
      prerequisites = null;

      var cast = Card.CanCast();
      var cycle = Card.CanCycle();

      if (cast.CanBeSatisfied && cycle.CanBeSatisfied)
      {
        cast.Description = String.Format("Cast {0}.", Card );
        cycle.Description = String.Format("{0}, Discard this card: Draw a card.", Card.CyclingCost);

        var dialog = _selectAbilityVmFactory.Create(new[] {cast, cycle});
        _shell.ShowModalDialog(dialog, DialogType.Large, SelectionMode.Disabled);

        if (dialog.WasCanceled)
          return false;

        if (dialog.Selected == cast)
        {
          isCast = true;
          prerequisites = cast;
          return true;
        }

        prerequisites = cycle;
        return true;
      }

      if (cast.CanBeSatisfied)
      {
        isCast = true;
        prerequisites = cast;
        return true;
      }

      prerequisites = cycle;
      return true;
    }

    private void Activate()
    {
      if (!IsPlayable)
        return;

      bool isCast;
      int? x = null;
      SpellPrerequisites prerequisites;
      var targets = new Targets();
      var payKicker = false;

      var success =
        SelectActivation(out isCast, out prerequisites) &&
          PayKicker(prerequisites, out payKicker) &&
            SelectX(prerequisites, out x) &&
              SelectTargets(prerequisites, payKicker, targets);

      if (!success)
        return;

      var playable = isCast
        ? (Playable) new Spell(Card, new ActivationParameters(targets, payKicker, x))
        : new Cyclable(Card);

      _publisher.Publish(new PlayableSelected {Playable = playable});
    }

    private bool SelectTargets(SpellPrerequisites prerequisites, bool payKicker, Targets targets)
    {
      var selectors = payKicker
        ? prerequisites.KickerTargetSelectors
        : prerequisites.TargetSelectors;

      if (selectors.HasCost)
      {
        var dialog = ShowSelectorDialog(selectors.Cost(0));

        if (dialog.WasCanceled)
          return false;

        targets.AddCost(dialog.Selection[0]);
      }
            
      if (selectors.HasEffect)
      {
        foreach (var selector in selectors.Effect())
        {
          var dialog = ShowSelectorDialog(selector);

          if (dialog.WasCanceled)
            return false;

          // this can occur when game is terminated
          if (dialog.Selection.Count == 0)
            return false;
         
          targets.AddEffect(dialog.Selection[0]);
        }
      }

      return true;
    }

    private SelectTarget.ViewModel ShowSelectorDialog(TargetSelector selector)
    {
      var dialog = _selectTargetVmFactory.Create(selector, canCancel: true,
        instructions: "(Press Esc to cancel.)");

      _shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);
      return dialog;
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
      _publisher.Publish(new TargetSelected {Target = Card});
    }

    private bool PayKicker(SpellPrerequisites prerequisites, out bool payKicker)
    {
      payKicker = false;

      if (prerequisites.CanCastWithKicker)
      {
        var result = _shell.ShowMessageBox(
          message: "Do you want to pay the kicker?",
          buttons: MessageBoxButton.YesNo,
          type: DialogType.Small);

        payKicker = result == MessageBoxResult.Yes;
      }

      return true;
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
    }
  }
}