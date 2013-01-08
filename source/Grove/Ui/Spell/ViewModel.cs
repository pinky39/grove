namespace Grove.Ui.Spell
{
  using System;
  using System.Linq;
  using Core;
  using Core.Cards;
  using Core.Decisions.Results;
  using Core.Targeting;
  using DistributeDamage;
  using Infrastructure;
  using Shell;

  public class ViewModel : CardViewModel, IReceive<UiInteractionChanged>, IReceive<TargetSelected>,
    IReceive<TargetUnselected>
  {
    private readonly Game _game;
    private readonly SelectAbility.ViewModel.IFactory _selectAbilityVmFactory;
    private readonly SelectTarget.ViewModel.IFactory _selectTargetVmFactory;
    private readonly SelectXCost.ViewModel.IFactory _selectXCostVmFactory;
    private readonly IShell _shell;
    private readonly UiDamageDistribution _uiDamageDistribution;
    private Action _select;

    public ViewModel(
      Card card,
      IShell shell,
      Game game,
      SelectTarget.ViewModel.IFactory selectTargetVmFactory,
      SelectXCost.ViewModel.IFactory selectXCostVmFactory,
      SelectAbility.ViewModel.IFactory selectAbilityVmFactory,
      UiDamageDistribution uiDamageDistribution) : base(card)
    {
      _shell = shell;
      _game = game;
      _selectTargetVmFactory = selectTargetVmFactory;
      _selectXCostVmFactory = selectXCostVmFactory;
      _selectAbilityVmFactory = selectAbilityVmFactory;
      _uiDamageDistribution = uiDamageDistribution;
      _select = delegate { };
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

          IsPlayable = Card.CanCast().Any(x => x.CanBeSatisfied) ||
            Card.CanActivateAbilities().Any(x => x.CanBeSatisfied);
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
      _game.Publish(new PlayersInterestChanged
        {
          Visual = Card
        });
    }

    public void Select()
    {
      _select();
    }

    private Ui.PlayableActivator SelectActivation()
    {
      var canCastSpells = Card.CanCast();
      var canActivateAbilities = Card.CanActivateAbilities();

      var activations = canCastSpells
        .Select((x, i) => new Ui.PlayableActivator
          {
            Prerequisites = x,
            GetPlayable = parameters => new Spell(Card, parameters, i)
          })
        .Where(x => x.Prerequisites.CanBeSatisfied).Concat(
          canActivateAbilities
            .Select((x, i) => new Ui.PlayableActivator
              {
                Prerequisites = x,
                GetPlayable = parameters => new Core.Decisions.Results.Ability(Card, parameters, i)
              })
            .Where(x => x.Prerequisites.CanBeSatisfied))
        .ToList();

      if (activations.Count == 1)
        return activations[0];

      var dialog = _selectAbilityVmFactory.Create(activations.Select(x => x.Prerequisites.Description));
      _shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

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
      _game.Publish(new PlayableSelected {Playable = playable});
    }

    private bool SelectTargets(SpellPrerequisites prerequisites, ActivationParameters parameters)
    {
      if (prerequisites.TargetSelector.RequiresCostTargets)
      {
        var dialog = ShowSelectorDialog(prerequisites.TargetSelector.Cost.FirstOrDefault());

        if (dialog.WasCanceled)
          return false;

        foreach (var target in dialog.Selection)
        {
          parameters.Targets.AddCost(target);
        }
      }

      if (prerequisites.TargetSelector.RequiresEffectTargets)
      {
        foreach (var selector in prerequisites.TargetSelector.Effect)
        {
          var dialog = ShowSelectorDialog(selector);

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

      if (prerequisites.DistributeDamage)
      {
        parameters.Targets.DamageDistributor = _uiDamageDistribution;
      }

      return true;
    }

    private SelectTarget.ViewModel ShowSelectorDialog(TargetValidator validator)
    {
      var dialog = _selectTargetVmFactory.Create(validator, canCancel: true,
        instructions: "(Press Spacebar when done, press Esc to cancel.)");

      _shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      return dialog;
    }

    private bool SelectX(SpellPrerequisites prerequisites, ActivationParameters parameters)
    {
      if (prerequisites.HasXInCost)
      {
        var dialog = _selectXCostVmFactory.Create(prerequisites.MaxX.Value);
        _shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.Disabled);

        if (dialog.WasCanceled)
          return false;

        parameters.X = dialog.ChosenX;
      }

      return true;
    }

    private void ChangeSelection()
    {
      _game.Publish(new SelectionChanged {Selection = Card});
    }    

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}