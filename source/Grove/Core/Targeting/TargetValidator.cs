namespace Grove.Core.Targeting
{
  using Core.Zones;
  using Costs;
  using Effects;

  public delegate bool TargetValidatorDelegate(TargetValidatorDelegateParameters parameters);
  public delegate bool ZoneValidatorDelegate(ZoneValidatorDelegateParameters parameters);

  public class TargetValidator : GameObject
  {
    private const string DefaultMessageOneTarget = "Select a target.";
    private const string DefaultMessageMultipleTargets = "Select target: {0} of {1}.";

    private readonly TargetValidatorDelegate _isValidTarget;
    private readonly ZoneValidatorDelegate _isValidZone;
    private readonly bool _mustBeTargetable;

    public TargetValidator(TargetValidatorParameters p)
    {
      _isValidTarget = p.TargetSpec;
      _isValidZone = p.ZoneSpec;
      _mustBeTargetable = p.MustBeTargetable;

      MinCount = p.MinCount;
      MaxCount = p.MaxCount;
      MessageFormat = p.Text;
    }

    public int? MaxCount { get; private set; }
    public int MinCount { get; private set; }

    public string MessageFormat { get; private set; }

    public virtual void Initialize(Game game)
    {
      Game = game;
    }

    public virtual bool IsTargetValid(ITarget target, Effect effect = null, Cost cost = null)
    {
      if (!CanBeTargeted(target, effect))
      {
        return false;
      }

      var parameters = new TargetValidatorDelegateParameters(target, Game);
      parameters.Effect = effect;
      parameters.Cost = cost;

      return _isValidTarget(parameters);
    }

    private bool CanBeTargeted(ITarget target, Effect effect)
    {
      if (!_mustBeTargetable)
        return true;

      if (!target.IsCard())
        return true;

      if (effect == null)
        return true;

      return target.Card().CanBeTargetBySpellsOwnedBy(effect.Controller) &&
        !target.Card().HasProtectionFrom(effect.Source.OwningCard);
    }

    public virtual string GetMessage(int targetNumber)
    {
      var messageFormat = MessageFormat;

      if (messageFormat == null)
      {
        messageFormat = MaxCount == 1
          ? DefaultMessageOneTarget
          : DefaultMessageMultipleTargets;
      }

      var maxNumber = MinCount == MaxCount
        ? MaxCount.ToString()
        : "max. " + MaxCount;

      return string.Format(messageFormat, targetNumber, maxNumber);
    }

    public bool HasValidZone(ITarget target)
    {
      var zone = target.Zone();
      
      if (zone == null)
        return true;
                  
      return _isValidZone(new ZoneValidatorDelegateParameters(zone.Value, target.Controller(), Game));
    }
  }
}