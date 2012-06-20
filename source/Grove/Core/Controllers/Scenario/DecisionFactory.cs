namespace Grove.Core.Controllers.Scenario
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Human;

  public class DecisionFactory : IHumanDecisionFactory
  {
    private readonly List<StepDecisions> _decisions = new List<StepDecisions>();
    private Game _game;

    public void Initialize(Game game)
    {
      _game = game;
    }

    public IDecision CreateAssignCombatDamage(Player player, Attacker attacker)
    {
      return new Machine.AssignCombatDamage
        {
          Player = player,
          Attacker = attacker
        };
    }

    public IDecision CreateConsiderPayingLifeOrMana(Player player, 
      string message, object context, PayLifeOrManaHandler handler,
                                                    int? life, IManaAmount mana)
    {
      return new Machine.ConsiderPayingLifeOrMana
        {
          Context = context,
          Player = player,
          Message = message,
          Game = _game,
          Handler = handler,
          Life = life,
          Mana = mana
        };
    }

    public IDecision CreateDeclareAttackers(Player player)
    {
      var decision = (DeclareAttackers) Next<DeclareAttackers>() ?? DeclareAttackers.None;
      decision.Combat = _game.Combat;

      return decision;
    }

    public IDecision CreateDeclareBlockers(Player player)
    {
      var decision = (DeclareBlockers) Next<DeclareBlockers>() ?? DeclareBlockers.None;
      decision.Combat = _game.Combat;
      return decision;
    }

    public IDecision CreateDiscardCards(Player player, int count)
    {
      return new Machine.DiscardCards
        {
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
      return new Machine.SacrificeCreatures
        {
          Player = player,
          Count = count
        };
    }

    public IDecision CreateSelectStartingPlayer(Player player)
    {
      return new Machine.SelectStartingPlayer
        {
          Player = player,
          Players = _game.Players
        };
    }

    public IDecision CreateSetDamageAssignmentOrder(Player player, Attacker attacker)
    {
      return new Machine.SetDamageAssignmentOrder
        {
          Player = player,
          Attacker = attacker
        };
    }

    public IDecision CreateSetTriggeredAbilityTarget(Player player, Effect effect, TargetSelector targetSelector)
    {
      var decision = (SetTriggeredAbilityTarget) Next<SetTriggeredAbilityTarget>() ?? SetTriggeredAbilityTarget.None;
      decision.Effect = effect;
      decision.Stack = _game.Stack;

      return decision;
    }

    public IDecision CreateTakeMulligan(Player player)
    {
      return new Machine.TakeMulligan
        {
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
        .Where(x => x.Step == _game.Turn.Step && x.Turn == _game.Turn.TurnCount)
        .SingleOrDefault();

      if (decisions == null)
        return null;

      return decisions.Next<TDecision>();
    }
  }
}