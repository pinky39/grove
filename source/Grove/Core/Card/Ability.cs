namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Grove.AI;
  using Grove.AI.CostRules;
  using Grove.AI.RepetitionRules;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Infrastructure;

  public abstract class Ability : GameObject, IEffectSource
  {
    private readonly Parameters _p;
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>(true);
    private Card _owner;

    protected Ability() { }

    protected Ability(Parameters parameters)
    {
      _p = parameters;
      Text = parameters.Text;
    }

    public CardText Text { get; private set; }
    public bool IsEnabled { get { return _isEnabled.Value; } set { _isEnabled.Value = value; } }
    public Card SourceCard { get { return _owner; } }
    public Card OwningCard { get { return _owner; } }
    public void EffectCountered(SpellCounterReason reason) { }
    public virtual int CalculateHash(HashCalculator calc) { return _isEnabled.Value.GetHashCode(); }
    void IEffectSource.EffectPushedOnStack() { }
    void IEffectSource.EffectResolved() { }
    bool IEffectSource.IsTargetStillValid(ITarget target, object triggerMessage) { return _p.TargetSelector.IsValidEffectTarget(target, triggerMessage); }

    bool IEffectSource.ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets)
    {
      return _p.TargetSelector.ValidateTargetDependencies(new ValidateTargetDependenciesParam
        {
          Cost = costTargets,
          Effect = effectTargets
        });
    }

    protected void Resolve(Effect e, bool skipStack)
    {
      if (_p.UsesStack == false || skipStack)
      {
        e.BeginResolve();
        e.FinishResolve();
        return;
      }

      Stack.Push(e);
    }

    public virtual void Initialize(Card owningCard, Game game)
    {
      _owner = owningCard;
      Game = game;

      _p.TargetSelector.Initialize(owningCard, game);

      foreach (var rule in _p.Rules)
      {
        rule.Initialize(game);
      }

      _isEnabled.Initialize(Game.ChangeTracker);
    }

    public override string ToString() { return string.Format("{0}'s ability", OwningCard); }

    [Copyable]
    public abstract class Parameters
    {
      public readonly List<MachinePlayRule> Rules = new List<MachinePlayRule>();
      public int DistributeAmount;
      public EffectFactory Effect;
      public TargetSelector TargetSelector = new TargetSelector();
      public string Text;
      public bool UsesStack = true;

      public bool HasTimingRules { get { return Rules.Any(x => x is TimingRule); } }

      public void TimingRule(TimingRule rule) { Rules.Add(rule); }
      public void RepetitionRule(RepetitionRule rule) { Rules.Add(rule); }
      public void TargetingRule(TargetingRule rule) { Rules.Add(rule); }
      public void CostRule(CostRule rule) { Rules.Add(rule); }
    }
  }

  
}