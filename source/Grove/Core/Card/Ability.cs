namespace Grove
{
  using System.Collections.Generic;
  using Effects;

  public abstract class Ability : GameObject, IEffectSource
  {
    private readonly AbilityParameters _p;
    private Card _owningCard;

    protected Ability() {}

    protected Ability(AbilityParameters parameters)
    {
      _p = parameters;
      Text = parameters.Text;
    }

    public CardText Text { get; private set; }

    public Card SourceCard
    {
      get { return _owningCard; }
    }

    public Card OwningCard
    {
      get { return _owningCard; }
    }

    public void EffectCountered(SpellCounterReason reason) {}
    void IEffectSource.EffectPushedOnStack() {}
    void IEffectSource.EffectResolved() {}

    bool IEffectSource.IsTargetStillValid(ITarget target, object triggerMessage)
    {
      return _p.TargetSelector.IsValidEffectTarget(target, triggerMessage);
    }

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
      _owningCard = owningCard;
      Game = game;

      _p.TargetSelector.Initialize(owningCard, game);

      foreach (var rule in _p.Rules)
      {
        rule.Initialize(game);
      }
    }

    public override string ToString()
    {
      return string.Format("{0}'s ability", OwningCard);
    }
  }
}