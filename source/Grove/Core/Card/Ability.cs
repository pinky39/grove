namespace Grove
{
  using System.Collections.Generic;
  using Effects;
  using Grove.Infrastructure;

  public abstract class Ability : GameObject, IEffectSource
  {
    private readonly AbilityParameters _p;
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>(true);
    private Card _owner;

    protected Ability() { }

    protected Ability(AbilityParameters parameters)
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
  }
}