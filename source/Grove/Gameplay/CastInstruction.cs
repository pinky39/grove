namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Infrastructure;
  using Grove.Gameplay.Messages;

  public class CastInstruction : GameObject, IEffectSource
  {
    private readonly CastingRule _castingRule;
    private readonly Cost _cost;
    private readonly CardText _description;
    private readonly int _distributeAmount;
    private readonly EffectFactory _effectFactory;
    private readonly List<MachinePlayRule> _rules;
    private readonly TargetSelector _targetSelector;
    private Card _card;

    private CastInstruction() {}

    public CastInstruction(CastInstructionParameters p)
    {
      _targetSelector = p.TargetSelector;
      _cost = p.Cost;
      _castingRule = p.Rule;
      _effectFactory = p.Effect;
      _description = p.Text;
      _distributeAmount = p.DistributeAmount;
      _rules = p.GetMachineRules();
    }

    public bool HasXInCost { get { return _cost.HasX; } }

    public Card OwningCard { get { return _card; } }
    public Card SourceCard { get { return _card; } }

    public void EffectCountered(SpellCounterReason reason)
    {
      _card.PutToGraveyard();

      Publish(new SpellWasCountered
        {
          Card = _card,
          Reason = reason
        });
    }

    public void EffectPushedOnStack()
    {
      _card.ChangeZone(
        destination: Stack, 
        add: delegate {});
    }

    public void EffectResolved()
    {
      _castingRule.AfterResolve();
    }

    public bool IsTargetStillValid(ITarget target, object triggerMessage)
    {
      return _targetSelector.IsValidEffectTarget(target, triggerMessage);
    }

    public bool ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets)
    {
      return _targetSelector.ValidateTargetDependencies(new ValidateTargetDependenciesParam
        {
          Cost = costTargets,
          Effect = effectTargets
        });
    }

    public int CalculateHash(HashCalculator calc)
    {
      return 0;
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;
      _card = card;

      _targetSelector.Initialize(card, game);

      foreach (var aiInstruction in _rules)
      {
        aiInstruction.Initialize(game);
      }

      _castingRule.Initialize(card, game);
      _cost.Initialize(card, game, _targetSelector.Cost.FirstOrDefault());
    }

    public bool CanCast(out ActivationPrerequisites prerequisites)
    {
      prerequisites = null;

      if (_castingRule.CanCast() == false)
      {
        return false;
      }

      var result = _cost.CanPay();      

      prerequisites = new ActivationPrerequisites
        {
          CanPay = result.CanPay(),
          Description = _description,
          Selector = _targetSelector,
          MaxX = result.MaxX(),
          DistributeAmount = _distributeAmount,
          Card = _card,
          Rules = _rules
        };

      return true;
    }

    public void Cast(ActivationParameters activationParameters)
    {
      var parameters = new EffectParameters
        {
          Source = this,
          Targets = activationParameters.Targets,
          X = activationParameters.X
        };

      var effect = _effectFactory().Initialize(parameters, Game);

      if (activationParameters.PayCost)
      {
        _cost.Pay(activationParameters.Targets, activationParameters.X);
      }

      if (activationParameters.SkipStack)
      {
        effect.Resolve();
        effect.FinishResolve();
        return;
      }

      _castingRule.Cast(effect);
    }

    public bool CanTarget(ITarget target)
    {
      return _targetSelector.Effect[0].IsTargetValid(target, _card);
    }

    public bool IsGoodTarget(ITarget target)
    {
      return TargetingHelper.IsGoodTarget(
        target, OwningCard, _targetSelector,
        _rules.Where(r => r is TargetingRule).Cast<TargetingRule>());
    }

    public IManaAmount GetManaCost()
    {
      return _cost.GetManaCost();
    }

    public override string ToString()
    {
      return _card.ToString();
    }
  }
}