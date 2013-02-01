namespace Grove.Core
{
  using System.Linq;
  using Ai;
  using Casting;
  using Costs;
  using Effects;
  using Infrastructure;
  using Mana;
  using Messages;
  using Targeting;
  using Zones;

  public class CastInstruction : GameObject, IEffectSource
  {
    private readonly CastingRule _castingRule;
    private readonly Cost _cost;
    private readonly string _description;
    private readonly bool _distributeDamage;
    private readonly EffectFactory _effectFactory;
    private readonly MachinePlayRule[] _rules;
    private readonly TargetSelector _targetSelector;
    private Card _card;

    private CastInstruction() {}

    public CastInstruction(CastInstructionParameters p)
    {
      _targetSelector = p.TargetSelector;
      _cost = p.Cost;
      _castingRule = p.Rule;
      _effectFactory = p.Effect;
      _description = p.Description;
      _distributeDamage = p.DistributeDamage;
      _rules = p.Rules;
    }

    public bool HasXInCost { get; private set; }

    public Card OwningCard { get { return _card; } }
    public Card SourceCard { get { return _card; } }

    public void EffectCountered(SpellCounterReason reason)
    {
      _card.PutToGraveyard();
    }

    public void EffectPushedOnStack()
    {
      _card.ChangeZone(Stack);

      Publish(new ZoneChanged
        {
          Card = _card,
          From = Zone.Hand,
          To = Zone.Stack
        });
    }

    public void EffectResolved()
    {
      _castingRule.AfterResolve();
    }

    public bool IsTargetStillValid(ITarget target)
    {
      return _targetSelector.IsValidEffectTarget(target);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return 0;
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;
      _card = card;

      _targetSelector.Initialize(game);

      foreach (var aiInstruction in _rules)
      {
        aiInstruction.Initialize(game);
      }
    }

    public bool CanCast(out ActivationPrerequisites prerequisites)
    {
      int? maxX = null;
      prerequisites = null;

      if (_castingRule.CanCast() == false || _cost.CanPay(ref maxX) == false)
      {
        return false;
      }

      prerequisites = new ActivationPrerequisites
        {
          Description = _description,
          Selector = _targetSelector,
          MaxX = maxX,
          DistributeDamage = _distributeDamage,
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

      var effect = _effectFactory(parameters, Game);

      if (activationParameters.PayCost)
      {
        _cost.Pay(activationParameters.Targets, activationParameters.X);
      }

      if (activationParameters.SkipStack)
      {
        effect.Resolve();
        return;
      }

      _castingRule.Cast(effect);
      Publish(new PlayerHasCastASpell(_card, activationParameters.Targets));
    }

    public bool CanTarget(ITarget target)
    {
      return _targetSelector.Effect[0].IsTargetValid(target);
    }

    public bool IsGoodTarget(ITarget target)
    {
      var generator = new TargetGenerator(
        _targetSelector,
        _card,
        Game,
        0);

      return generator.Any(targets => targets.Effect.Contains(target));
    }

    public IManaAmount GetManaCost()
    {
      return _cost.GetManaCost();
    }
  }
}