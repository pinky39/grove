namespace Grove.Core.Controllers.Scenario
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Human;
  using Zones;

  public class DecisionFactory : IHumanDecisionFactory
  {
    private readonly Combat _combat;
    private readonly List<StepDecisions> _decisions = new List<StepDecisions>();
    private readonly Players _players;
    private readonly Stack _stack;
    private readonly TurnInfo _turn;

    public DecisionFactory(TurnInfo turn, Players players, Stack stack, Combat combat)
    {
      _turn = turn;
      _stack = stack;
      _combat = combat;
      _players = players;
    }

    public IDecision CreateAssignCombatDamage(Player player, Attacker attacker)
    {
      return new Machine.AssignCombatDamage{
        Player = player,
        Attacker = attacker
      };
    }

    public IDecision CreateConsiderPayingLifeOrMana(Player player, Effect effect, PayLifeOrManaHandler handler, int? life, IManaAmount mana)
    {
      return new Machine.ConsiderPayingLifeOrMana{
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
      var decision = (DeclareAttackers) Next<DeclareAttackers>() ?? DeclareAttackers.None;
      decision.Combat = _combat;

      return decision;
    }

    public IDecision CreateDeclareBlockers(Player player)
    {
      var decision = (DeclareBlockers) Next<DeclareBlockers>() ?? DeclareBlockers.None;
      decision.Combat = _combat;
      return decision;
    }

    public IDecision CreateDiscardCards(Player player, int count)
    {
      return new Machine.DiscardCards{
        Player = player,
        Count = count
      };
    }

    public IDecision CreatePlaySpellOrAbility(Player player)
    {
      return Next<PlaySpellOrAbility>() ?? PlaySpellOrAbility.Pass;
    }

    public IDecision CreateSacrificeCreatures(Player player, int count)
    {
      return new Machine.SacrificeCreatures{
        Player = player,
        Count = count
      };
    }

    public IDecision CreateSelectStartingPlayer(Player player)
    {
      return new Machine.SelectStartingPlayer{
        Player = player,
        Players = _players
      };
    }

    public IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker)
    {
      return new Machine.SetDamageAssignmentOrder{
        Player = player,
        Attacker = attacker
      };
    }

    public IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelector targetSelector)
    {
      var decision = (SetTriggeredAbilityTarget) Next<SetTriggeredAbilityTarget>() ?? SetTriggeredAbilityTarget.None;
      decision.Effect = effect;
      decision.Stack = _stack;

      return decision;
    }

    public IDecision CreateTakeMulligan(Player player)
    {
      return new Machine.TakeMulligan{
        Player = player
      };
    }

    public void AddDecisions(IEnumerable<StepDecisions> decisions)
    {
      _decisions.AddRange(decisions);
    }

    private IDecision Next<TDecision>()
    {
      var decisions = _decisions
        .Where(x => x.Step == _turn.Step && x.Turn == _turn.TurnCount)
        .SingleOrDefault();

      if (decisions == null)
        return null;

      return decisions.Next<TDecision>();
    }
  }
}