namespace Grove.Gameplay.Abilities
{
  using System.Collections.Generic;
  using Artifical;
  using Characteristics;
  using Effects;
  using Infrastructure;
  using Misc;
  using Targeting;

  public abstract class Ability : GameObject, IEffectSource
  {
    protected readonly TargetSelector TargetSelector;
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>(true);
    private readonly bool _usesStack;
    protected int DistributeAmount;
    protected EffectFactory EffectFactory;
    protected List<MachinePlayRule> Rules;
    private Card _owner;

    protected Ability() {}

    protected Ability(AbilityParameters parameters)
    {
      EffectFactory = parameters.Effect;
      Rules = parameters.GetMachineRules();
      TargetSelector = parameters.TargetSelector;
      DistributeAmount = parameters.DistributeAmount;
      _usesStack = parameters.UsesStack;
      Text = parameters.Text;
    }

    public CardText Text { get; private set; }

    public bool IsEnabled { get { return _isEnabled.Value; } private set { _isEnabled.Value = value; } }
    public Card SourceCard { get { return _owner; } }
    public Card OwningCard { get { return _owner; } }

    public void EffectCountered(SpellCounterReason reason) {}

    public virtual int CalculateHash(HashCalculator calc)
    {
      return _isEnabled.Value.GetHashCode();
    }

    void IEffectSource.EffectPushedOnStack() {}
    void IEffectSource.EffectResolved() {}

    bool IEffectSource.IsTargetStillValid(ITarget target, object triggerMessage)
    {
      return TargetSelector.IsValidEffectTarget(target, triggerMessage);
    }

    public void Enable()
    {
      IsEnabled = true;
    }

    public void Disable()
    {
      IsEnabled = false;
    }

    protected void Resolve(Effect e, bool skipStack)
    {
      if (_usesStack == false || skipStack)
      {
        e.Resolve();
        e.FinishResolve();
        return;
      }

      Stack.Push(e);
    }

    public virtual void Initialize(Card owningCard, Game game)
    {
      _owner = owningCard;
      Game = game;

      TargetSelector.Initialize(owningCard, game);

      foreach (var rule in Rules)
      {
        rule.Initialize(game);
      }

      _isEnabled.Initialize(Game.ChangeTracker);
    }

    public override string ToString()
    {
      return string.Format("{0}'s ability", OwningCard);
    }
  }
}