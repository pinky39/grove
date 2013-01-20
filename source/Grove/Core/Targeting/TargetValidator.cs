namespace Grove.Core.Targeting
{
  using Core.Zones;
  using Costs;
  using Effects;

  public delegate bool IsValidTarget(IsValidTargetParameters parameters);

  public delegate bool IsValidZone(IsValidZoneParameters parameters);


  public class TargetValidator : GameObject
  {
    private const string DefaultMessageOneTarget = "Select a target.";
    private const string DefaultMessageMultipleTargets = "Select target: {0} of {1}.";

    private readonly IsValidTarget _isValidTarget;
    private readonly IsValidZone _isValidZone;
    private readonly bool _mustBeTargetable;

    public TargetValidator(TargetValidatorParameters p)
    {
      _isValidTarget = p.IsValidTarget;
      _isValidZone = p.IsValidZone;
      _mustBeTargetable = p.MustBeTargetable;

      MinCount = p.MinCount;
      MaxCount = p.MaxCount;
      MessageFormat = p.MessageFormat;
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

      var parameters = new IsValidTargetParameters(target, Game);
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

    public bool IsZoneValid(Zone zone, Player zoneOwner)
    {
      return _isValidZone(new IsValidZoneParameters(zone, zoneOwner, Game));
    }
  }
}