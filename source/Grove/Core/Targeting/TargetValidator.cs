namespace Grove.Core.Targeting
{
  using Core.Zones;
  using Modifiers;

  public delegate bool TargetValidatorDelegate(TargetValidatorDelegateParameters parameters);

  public delegate bool ZoneValidatorDelegate(ZoneValidatorDelegateParameters parameters);

  public class TargetValidator : GameObject
  {
    private const string DefaultMessageOneTarget = "Select a target.";
    private const string DefaultMessageMultipleTargets = "Select target: {0} of {1}.";

    private readonly TargetValidatorDelegate _isValidTarget;
    private readonly ZoneValidatorDelegate _isValidZone;
    private readonly bool _mustBeTargetable;
    private Card _owningCard;

    private TargetValidator() {}

    public TargetValidator(TargetValidatorParameters p)
    {
      _isValidTarget = p.TargetSpec;
      _isValidZone = p.ZoneSpec;
      _mustBeTargetable = p.MustBeTargetable;

      MinCount = p.MinCount;
      MaxCount = p.MaxCount;
      MessageFormat = p.Text;
    }

    public Value MaxCount { get; private set; }
    public Value MinCount { get; private set; }

    public string MessageFormat { get; private set; }

    public virtual void Initialize(Card owningCard, Game game)
    {
      Game = game;
      _owningCard = owningCard;
    }

    public virtual bool IsTargetValid(ITarget target, object triggerMessage = null)
    {
      if (!CanBeTargeted(target, _owningCard))
      {
        return false;
      }

      var parameters = new TargetValidatorDelegateParameters
        {
          Game = Game,
          OwningCard = _owningCard,
          Target = target
        };

      parameters.SetTriggerMessage(triggerMessage);

      return _isValidTarget(parameters);
    }

    public virtual bool IsZoneValid(Zone zone, Player controller)
    {
      var p = new ZoneValidatorDelegateParameters(zone, _owningCard, controller, Game);

      return _isValidZone(p);
    }

    private bool CanBeTargeted(ITarget target, Card owningCard)
    {
      if (!_mustBeTargetable)
        return true;

      if (!target.IsCard())
        return true;


      return target.Card().CanBeTargetBySpellsOwnedBy(owningCard.Controller) &&
        !target.Card().HasProtectionFrom(owningCard);
    }

    public virtual string GetMessage(int targetNumber, int? x)
    {
      var messageFormat = MessageFormat;

      if (messageFormat == null)
      {
        messageFormat = MaxCount.GetValue(x) == 1
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

      return _isValidZone(new ZoneValidatorDelegateParameters(zone.Value, _owningCard, target.Controller(), Game));
    }
  }
}