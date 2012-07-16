namespace Grove.Core.Controllers.Human
{
  using System;
  using Details.Cards.Effects;
  using Details.Combat;
  using Details.Mana;
  using Targeting;
  using Ui.CombatDamage;
  using Ui.Shell;

  //public interface IHumanDecisionFactory : IDecisionFactory {}

  //public class DecisionFactory : IHumanDecisionFactory
  //{
  //  private readonly ViewModel.IFactory _combatVmFactory;
  //  private readonly Configuration _configuration;
  //  private readonly Ui.DamageOrder.ViewModel.IFactory _damageOrderVmFactory;
  //  private readonly Ui.Priority.ViewModel.IFactory _priorityVmFactory;
  //  private readonly Ui.SelectTarget.ViewModel.IFactory _selectTargetVmFactory;
  //  private readonly IShell _shell;
  //  private Game _game;

  //  public DecisionFactory(
  //    Ui.SelectTarget.ViewModel.IFactory selectTargetVmFactory,
  //    ViewModel.IFactory combatVmFactory,
  //    Ui.DamageOrder.ViewModel.IFactory damageOrderVmFactory,
  //    Ui.Priority.ViewModel.IFactory priorityVmFactory,
  //    IShell shell,
  //    Configuration configuration)
  //  {
  //    _selectTargetVmFactory = selectTargetVmFactory;
  //    _damageOrderVmFactory = damageOrderVmFactory;
  //    _priorityVmFactory = priorityVmFactory;
  //    _configuration = configuration;
  //    _combatVmFactory = combatVmFactory;
  //    _shell = shell;
  //  }

  //  public void Initialize(Game game)
  //  {
  //    _game = game;
  //  }

  //  public IDecision CreateAssignCombatDamage(Player player, Attacker attacker)
  //  {
  //    return new AssignCombatDamage
  //      {
  //        Player = player,
  //        Attacker = attacker,
  //        DialogFactory = _combatVmFactory,
  //        Shell = _shell
  //      };
  //  }

  //  public IDecision CreateDeclareAttackers(Player player)
  //  {
  //    return new DeclareAttackers
  //      {
  //        Player = player,
  //        Combat = _game.Combat,
  //        DialogFactory = _selectTargetVmFactory,
  //        Shell = _shell,
  //        Publisher = _game.Publisher
  //      };
  //  }

  //  public IDecision CreateDeclareBlockers(Player player)
  //  {
  //    return new DeclareBlockers
  //      {
  //        Player = player,
  //        Combat = _game.Combat,
  //        DialogFactory = _selectTargetVmFactory,
  //        Shell = _shell,
  //        Publisher = _game.Publisher
  //      };
  //  }

  //  public IDecision CreateDiscardCards(Player player, int count)
  //  {
  //    return new DiscardCards
  //      {
  //        Player = player,
  //        Count = count,
  //        DialogFactory = _selectTargetVmFactory,
  //        Shell = _shell
  //      };
  //  }

  //  public IDecision CreatePlaySpellOrAbility(Player player)
  //  {
  //    return new PlaySpellOrAbility
  //      {
  //        Player = player,
  //        Stack = _game.Stack,
  //        Configuration = _configuration,
  //        DialogFactory = _priorityVmFactory,
  //        Shell = _shell,
  //        Turn = _game.Turn,
  //      };
  //  }

  //  public IDecision CreateSacrificeCreatures(Player player, int count)
  //  {
  //    return new SacrificeCreatures
  //      {
  //        Player = player,
  //        Count = count,
  //        DialogFactory = _selectTargetVmFactory,
  //        Shell = _shell
  //      };
  //  }

  //  public IDecision CreateSelectStartingPlayer(Player player)
  //  {
  //    return new SelectStartingPlayer
  //      {
  //        Player = player,
  //        Players = _game.Players,
  //        Shell = _shell
  //      };
  //  }

  //  public IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker)
  //  {
  //    return new SetDamageAssignmentOrder
  //      {
  //        Player = player,
  //        Attacker = attacker,
  //        DialogFactory = _damageOrderVmFactory,
  //        Shell = _shell
  //      };
  //  }

  //  public IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelectors targetSelectors)
  //  {
  //    return new SetTriggeredAbilityTarget
  //      {
  //        Player = player,
  //        Effect = effect,
  //        TargetSelectors = targetSelectors,
  //        DialogFactory = _selectTargetVmFactory,
  //        Shell = _shell,
  //        Stack = _game.Stack
  //      };
  //  }

  //  public IDecision CreateTakeMulligan(Player player)
  //  {
  //    return new TakeMulligan
  //      {
  //        Player = player,
  //        Shell = _shell
  //      };
  //  }

  //  public IDecision CreateConsiderPayingLifeOrMana(Player player, string message, object context,
  //                                                  PayLifeOrManaHandler handler,
  //                                                  int? life, IManaAmount mana)
  //  {
  //    return new ConsiderPayingLifeOrMana
  //      {
  //        Player = player,
  //        Context = context,
  //        Message = message,
  //        Handler = handler,
  //        Life = life,
  //        Mana = mana,
  //        Shell = _shell,
  //        Game = _game,
  //      };
  //  }    
  //}
}