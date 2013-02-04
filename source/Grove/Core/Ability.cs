namespace Grove.Core
{
  using System.Collections.Generic;
  using Ai;
  using Effects;
  using Infrastructure;
  using Targeting;

  public abstract class Ability : GameObject, IEffectSource
  {
    protected readonly TargetSelector TargetSelector;
    protected readonly CardText Text;
    private readonly bool _usesStack;
    protected EffectFactory EffectFactory;
    protected List<MachinePlayRule> Rules;
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>(true);
    private Card _owner;
    protected int DistributeAmount;

    protected Ability(AbilityParameters parameters)
    {
      EffectFactory = parameters.Effect;
      Rules = parameters.GetMachineRules();
      TargetSelector = parameters.TargetSelector;
      DistributeAmount = parameters.DistributeAmount;
      _usesStack = parameters.UsesStack;
      Text = parameters.Text;
    }

    public bool IsEnabled { get { return _isEnabled.Value; } set { _isEnabled.Value = value; } }
    public Card SourceCard { get { return _owner; } }
    public Card OwningCard { get { return _owner; } }
    public void EffectCountered(SpellCounterReason reason) {}

    public virtual int CalculateHash(HashCalculator calc)
    {
      return _isEnabled.GetHashCode();
    }

    void IEffectSource.EffectPushedOnStack() {}
    void IEffectSource.EffectResolved() {}

    bool IEffectSource.IsTargetStillValid(ITarget target)
    {
      return TargetSelector.IsValidEffectTarget(target);
    }

    protected void Resolve(Effect e, bool skipStack)
    {
      if (_usesStack == false || skipStack)
      {
        e.Resolve();  
        return;
      }

      Game.Stack.Push(e);      
    }

    public virtual void Initialize(Card owner, Game game)
    {
      _owner = owner;
      Game = game;

      TargetSelector.Initialize(game);

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