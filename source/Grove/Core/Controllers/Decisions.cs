namespace Grove.Core.Controllers
{
  using Ai;
  using Effects;
  using Human;
  using Infrastructure;
  using Machine;

  [Copyable]
  public class Decisions
  {
    private readonly DecisionQueue _decisionQueue;
    private readonly IHumanDecisionFactory _humanDecisionFactory;
    private readonly IMachineDecisionFactory _machineDecisionFactory;
    private readonly Players _players;
    private readonly Search _search;

    private Decisions() {}

    public Decisions(
      DecisionQueue decisionQueue,
      Search search,
      Players players,
      IHumanDecisionFactory humanDecisionFactory,
      IMachineDecisionFactory machineDecisionFactory)
    {
      _decisionQueue = decisionQueue;
      _machineDecisionFactory = machineDecisionFactory;
      _humanDecisionFactory = humanDecisionFactory;
      _search = search;
      _players = players;
    }

    public void EnqueueAssignCombatDamage(Player player, Attacker attacker)
    {
      var decision = SelectDecisionFactory(player)
        .CreateAssignCombatDamage(player, attacker);

      Enqueue(decision);
    }

    public void EnqueueConsiderPayingLifeOrMana(Player player, Effect effect, PayLifeOrManaHandler handler, int? life = null, ManaAmount mana = null)
    {
      var decision = SelectDecisionFactory(player)
        .CreateConsiderPayingLifeOrMana(player, effect, handler, life, mana);

      Enqueue(decision);
    }

    public void EnqueueDeclareAttackers(Player player)
    {
      var decision = SelectDecisionFactory(player)
        .CreateDeclareAttackers(player);

      Enqueue(decision);
    }

    public void EnqueueDeclareBlockers(Player player)
    {
      var decision = SelectDecisionFactory(player)
        .CreateDeclareBlockers(player);

      Enqueue(decision);
    }

    public void EnqueueDiscardCards(Player player, int count)
    {
      var decision = SelectDecisionFactory(player)
        .CreateDiscardCards(player, count);

      Enqueue(decision);
    }

    public void EnqueuePlaySpellOrAbility(Player player)
    {
      _players.SetPriority(player);

      var decision = SelectDecisionFactory(player)
        .CreatePlaySpellOrAbility(player);

      Enqueue(decision);
    }

    public void EnqueueSacrificeCreatures(Player player, int count)
    {
      var decision = SelectDecisionFactory(player)
        .CreateSacrificeCreatures(player, count);

      Enqueue(decision);
    }

    public void EnqueueSelectStartingPlayer(Player player)
    {
      var decision = SelectDecisionFactory(player).
        CreateSelectStartingPlayer(player);

      Enqueue(decision);
    }

    public void EnqueueSetDamageAssignmentOrder(Player player, Attacker attacker)
    {
      var decision = SelectDecisionFactory(player)
        .CreateSetDamageAssignmentOrder(player, attacker);

      Enqueue(decision);
    }

    public void EnqueueSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelector targetSelector)
    {
      var decision = SelectDecisionFactory(player)
        .CreateSetTriggeredAbilityTarget(player, effect, targetSelector);

      Enqueue(decision);
    }

    public void EnqueueTakeMulligan(Player player)
    {
      var decision = SelectDecisionFactory(player)
        .CreateTakeMulligan(player);

      Enqueue(decision);
    }

    public void Initialize(Game game)
    {
      _machineDecisionFactory.Initialize(game);
    }

    public IDecisionFactory SelectDecisionFactory(Player player)
    {
      if (_search.InProgress)
        return _machineDecisionFactory;

      return player.IsHuman ? (IDecisionFactory) _humanDecisionFactory : _machineDecisionFactory;
    }

    private void Enqueue(IDecision decision)
    {
      _decisionQueue.Enqueue(decision);
    }
  }
}