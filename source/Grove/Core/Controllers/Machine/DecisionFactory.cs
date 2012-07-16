namespace Grove.Core.Controllers.Machine
{
  using Ai;
  using Details.Cards.Effects;
  using Details.Combat;
  using Details.Mana;
  using Infrastructure;
  using Targeting;

  //public interface IMachineDecisionFactory : IDecisionFactory {}

  //[Copyable]
  //public class DecisionFactory : IMachineDecisionFactory
  //{
  //  private readonly CastRestrictions _castRestrictions;
  //  private Game _game;

  //  private DecisionFactory() {}

  //  public DecisionFactory(
  //    CastRestrictions castRestrictions)
  //  {
  //    _castRestrictions = castRestrictions;
  //  }

  //  public IDecision CreateAssignCombatDamage(Player player, Attacker attacker)
  //  {
  //    return new AssignCombatDamage
  //      {
  //        Player = player,
  //        Attacker = attacker
  //      };
  //  }

  //  public IDecision CreateConsiderPayingLifeOrMana(
  //    Player player, string message, object context, PayLifeOrManaHandler handler,
  //    int? life, IManaAmount mana)
  //  {
  //    return new ConsiderPayingLifeOrMana
  //      {
  //        Context = context,
  //        Player = player,
  //        Message = message,
  //        Handler = handler,
  //        Life = life,
  //        Mana = mana
  //      };
  //  }

  //  public IDecision CreateDeclareAttackers(Player player)
  //  {
  //    return new DeclareAttackers(_game.ChangeTracker)
  //      {
  //        Player = player,
  //        Combat = _game.Combat,
  //        Game = _game,
  //        Search = _game.Search,
  //      };
  //  }

  //  public IDecision CreateDeclareBlockers(Player player)
  //  {
  //    return new DeclareBlockers
  //      {
  //        Player = player,
  //        Combat = _game.Combat
  //      };
  //  }

  //  public IDecision CreateDiscardCards(Player player, int count)
  //  {
  //    return new DiscardCards
  //      {
  //        Player = player,
  //        Count = count
  //      };
  //  }

  //  public IDecision CreatePlaySpellOrAbility(Player player)
  //  {
  //    return new PlaySpellOrAbility(_game.ChangeTracker)
  //      {
  //        Player = player,
  //        Search = _game.Search,
  //        Game = _game,
  //        Restrictions = _castRestrictions
  //      };
  //  }

  //  public IDecision CreateSacrificeCreatures(Player player, int count)
  //  {
  //    return new SacrificeCreatures
  //      {
  //        Player = player,
  //        Count = count
  //      };
  //  }

  //  public IDecision CreateSelectStartingPlayer(Player player)
  //  {
  //    return new SelectStartingPlayer
  //      {
  //        Player = player,
  //        Players = _game.Players,
  //      };
  //  }

  //  public IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker)
  //  {
  //    return new SetDamageAssignmentOrder
  //      {
  //        Player = player,
  //        Attacker = attacker
  //      };
  //  }

  //  public IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelectors targetSelectors)
  //  {
  //    return new SetTriggeredAbilityTarget(_game.ChangeTracker)
  //      {
  //        Player = player,
  //        Effect = effect,
  //        TargetSelectors = targetSelectors,
  //        Game = _game,
  //        Search = _game.Search,
  //        Stack = _game.Stack
  //      };
  //  }

  //  public IDecision CreateTakeMulligan(Player player)
  //  {
  //    return new TakeMulligan
  //      {
  //        Player = player
  //      };
  //  }

  //  public void Initialize(Game game)
  //  {
  //    _game = game;
  //  }
  //}
}