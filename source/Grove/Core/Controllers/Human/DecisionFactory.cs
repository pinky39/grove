namespace Grove.Core.Controllers.Human
{
  using Effects;
  using Infrastructure;
  using Ui.CombatDamage;
  using Ui.Shell;
  using Zones;

  public interface IHumanDecisionFactory : IDecisionFactory {}

  public class DecisionFactory : IHumanDecisionFactory
  {
    private readonly Combat _combat;
    private readonly ViewModel.IFactory _combatVmFactory;
    private readonly Configuration _configuration;
    private readonly Ui.DamageOrder.ViewModel.IFactory _damageOrderVmFactory;
    private readonly Players _players;
    private readonly Publisher _publisher;
    private readonly Ui.Priority.ViewModel.IFactory _priorityVmFactory;
    private readonly Ui.SelectTarget.ViewModel.IFactory _selectTargetVmFactory;
    private readonly IShell _shell;
    private readonly Stack _stack;
    private readonly TurnInfo _turn;

    public DecisionFactory(
      Ui.SelectTarget.ViewModel.IFactory selectTargetVmFactory,
      ViewModel.IFactory combatVmFactory,
      Ui.DamageOrder.ViewModel.IFactory damageOrderVmFactory,
      Ui.Priority.ViewModel.IFactory priorityVmFactory,
      IShell shell,
      Combat combat,
      Stack stack,
      Configuration configuration,
      TurnInfo turn,
      Players players, 
      Publisher publisher)
    {
      _selectTargetVmFactory = selectTargetVmFactory;
      _damageOrderVmFactory = damageOrderVmFactory;
      _players = players;
      _publisher = publisher;
      _turn = turn;
      _priorityVmFactory = priorityVmFactory;
      _configuration = configuration;
      _stack = stack;
      _combatVmFactory = combatVmFactory;
      _shell = shell;
      _combat = combat;
    }

    public IDecision CreateAssignCombatDamage(Player player, Attacker attacker)
    {
      return new AssignCombatDamage{
        Player = player,
        Attacker = attacker,
        DialogFactory = _combatVmFactory,
        Shell = _shell
      };
    }

    public IDecision CreateDeclareAttackers(Player player)
    {
      return new DeclareAttackers{
        Player = player,
        Combat = _combat,
        DialogFactory = _selectTargetVmFactory,
        Shell = _shell,
        Publisher = _publisher
      };
    }

    public IDecision CreateDeclareBlockers(Player player)
    {
      return new DeclareBlockers{
        Player = player,
        Combat = _combat,
        DialogFactory = _selectTargetVmFactory,
        Shell = _shell,
        Publisher = _publisher
      };
    }

    public IDecision CreateDiscardCards(Player player, int count)
    {
      return new DiscardCards{
        Player = player,
        Count = count,
        DialogFactory = _selectTargetVmFactory,
        Shell = _shell
      };
    }

    public IDecision CreatePlaySpellOrAbility(Player player)
    {
      return new PlaySpellOrAbility{
        Player = player,
        Stack = _stack,
        Configuration = _configuration,
        DialogFactory = _priorityVmFactory,
        Shell = _shell,
        Turn = _turn,
      };
    }

    public IDecision CreateSacrificeCreatures(Player player, int count)
    {
      return new SacrificeCreatures{
        Player = player,
        Count = count,
        DialogFactory = _selectTargetVmFactory,
        Shell = _shell
      };
    }

    public IDecision CreateSelectStartingPlayer(Player player)
    {
      return new SelectStartingPlayer{
        Player = player,
        Players = _players,
        Shell = _shell
      };
    }

    public IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker)
    {
      return new SetDamageAssignmentOrder{
        Player = player,
        Attacker = attacker,
        DialogFactory = _damageOrderVmFactory,
        Shell = _shell
      };
    }

    public IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelector targetSelector)
    {
      return new SetTriggeredAbilityTarget{
        Player = player,
        Effect = effect,
        TargetSelector = targetSelector,
        DialogFactory = _selectTargetVmFactory,
        Shell = _shell,
        Stack = _stack
      };
    }

    public IDecision CreateTakeMulligan(Player player)
    {
      return new TakeMulligan{
        Player = player,
        Shell = _shell
      };
    }

    public IDecision CreateConsiderPayingLifeOrMana(Player player, Effect effect, PayLifeOrManaHandler handler, int? life, IManaAmount mana)
    {
      return new ConsiderPayingLifeOrMana{
        Player = player,
        Effect = effect,
        Handler = handler,
        Life = life,
        Mana = mana,
        Shell = _shell,
        Stack = _stack
      };
    }

  }
}