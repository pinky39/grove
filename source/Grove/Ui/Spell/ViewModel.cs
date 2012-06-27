namespace Grove.Ui.Spell
{
  using System;
  using System.Linq;
  using System.Windows;
  using Core;
  using Core.Controllers.Results;
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
          IsPlayable = Card.Controller.HasPriority
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

    private SpellPrerequisites SelectActivation()
    {
      var prerequisites = new[] {Card.CanCast(), Card.CanCycle()};
      var canActivateCount = prerequisites.Count(x => x.CanBeSatisfied);

      switch (canActivateCount)
      {
        case (0):
          return null;
        case (1):
          return prerequisites.First(x => x.CanBeSatisfied);
        default:
          {
            prerequisites[0].Description = String.Format("Cast {0}.", Card);
            prerequisites[1].Description = String.Format("{{{0}}}, Discard this card: Draw a card.)", Card.CyclingCost);

            var dialog = _selectAbilityVmFactory.Create(prerequisites);
            _shell.ShowModalDialog(dialog, DialogType.Large, SelectionMode.Disabled);

            return dialog.WasCanceled ? null : dialog.Selected;
          }
      }
    }

    private void Activate()
    {
      var prerequisites = SelectActivation();

      if (prerequisites == null)
        return;

      var activation = new ActivationParameters();

      if (prerequisites.CanCastWithKicker)
        activation.PayKicker = PayKicker();

      if (prerequisites.HasXInCost)
      {
        var dialog = _selectXCostVmFactory.Create(prerequisites.MaxX.Value);
        _shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.Disabled);

        if (dialog.WasCanceled)
          return;

        activation.X = dialog.ChosenX;
      }

      var needsTargets = activation.PayKicker
        ? prerequisites.NeedsKickerEffectTargets
        : prerequisites.NeedsEffectTargets;

      if (needsTargets)
      {
        ITargetSelector selector = activation.PayKicker
          ? prerequisites.KickerTargetSelector
          : prerequisites.EffectTargetSelector;

        var dialog = _selectTargetVmFactory.Create(
          selector,
          canCancel: true,
          instructions: "(Press Esc to cancel.)");

        _shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

        if (dialog.WasCanceled)
          return;

        activation.Target = dialog.Selection.Single();
      }

      var spell = new Spell(Card, activation);
      _publisher.Publish(new PlayableSelected {Playable = spell});
    }

    private void MarkAsTarget()
    {
      _publisher.Publish(new TargetSelected {Target = Card});
    }

    private bool PayKicker()
    {
      var result = _shell.ShowMessageBox(
        message: "Do you want to pay the kicker?",
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      return result == MessageBoxResult.Yes;
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
    }
  }
}