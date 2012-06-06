namespace Grove.Core.Controllers.Machine
{
  using Ai;
  using Effects;
  using Infrastructure;
  using Zones;

  public interface IMachineDecisionFactory : IDecisionFactory
  {
    void Initialize(Game game);
  }

  [Copyable]
  public class DecisionFactory : IMachineDecisionFactory
  {
    private readonly CastRestrictions _castRestrictions;
    private readonly ChangeTracker _changeTracker;
    private readonly Combat _combat;
    private readonly Players _players;
    private readonly Search _search;
    private readonly Stack _stack;
    private Game _game;

    private DecisionFactory() {}

    public DecisionFactory(
      Combat combat,
      Search search,
      CastRestrictions castRestrictions,
      Players players,
      Stack stack,
      ChangeTracker changeTracker)
    {
      _combat = combat;
      _stack = stack;
      _changeTracker = changeTracker;
      _players = players;
      _search = search;
      _castRestrictions = castRestrictions;
    }

    public IDecision CreateAssignCombatDamage(Player player, Attacker attacker)
    {
      return new AssignCombatDamage{
        Player = player,
        Attacker = attacker
      };
    }

    public IDecision CreateConsiderPayingLifeOrMana(Player player, Effect effect, PayLifeOrManaHandler handler, int? life, IManaAmount mana)
    {
      return new ConsiderPayingLifeOrMana{
        Effect = effect,
        Player = player,
        Stack = _stack,
        Handler = handler,
        Life = life,
        Mana = mana
      };
    }

    public IDecision CreateDeclareAttackers(Player player)
    {
      return new DeclareAttackers(_changeTracker){
        Player = player,
        Combat = _combat,
        Game = _game,
        Search = _search,
      };
    }

    public IDecision CreateDeclareBlockers(Player player)
    {
      return new DeclareBlockers{
        Player = player,
        Combat = _combat
      };
    }

    public IDecision CreateDiscardCards(Player player, int count)
    {
      return new DiscardCards{
        Player = player,
        Count = count
      };
    }

    public IDecision CreatePlaySpellOrAbility(Player player)
    {
      return new PlaySpellOrAbility(_changeTracker){
        Player = player,
        Search = _search,
        Game = _game,
        Restrictions = _castRestrictions
      };
    }

    public IDecision CreateSacrificeCreatures(Player player, int count)
    {
      return new SacrificeCreatures{
        Player = player,
        Count = count
      };
    }

    public IDecision CreateSelectStartingPlayer(Player player)
    {
      return new SelectStartingPlayer{
        Player = player,
        Players = _players,
      };
    }

    public IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker)
    {
      return new SetDamageAssignmentOrder{
        Player = player,
        Attacker = attacker
      };
    }

    public IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelector targetSelector)
    {
      return new SetTriggeredAbilityTarget(_changeTracker){
        Player = player,
        Effect = effect,
        TargetSelector = targetSelector,
        Game = _game,
        Search = _search,
        Stack = _stack
      };
    }

    public IDecision CreateTakeMulligan(Player player)
    {
      return new TakeMulligan{
        Player = player
      };
    }

    public void Initialize(Game game)
    {
      _game = game;
    }
  }
}