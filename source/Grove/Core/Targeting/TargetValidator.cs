namespace Grove.Core.Targeting
{
  using System;
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
    private Player _controller;
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
      Message = p.Message;
    }

    public Value MaxCount { get; private set; }
    public Value MinCount { get; private set; }

    public string Message { get; private set; }

    public virtual void Initialize(Game game, Player controller, Card owningCard = null)
    {
      Game = game;

      _owningCard = owningCard;
      _controller = controller;
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
          Controller = _controller,
          Target = target
        };

      parameters.SetTriggerMessage(triggerMessage);

      return _isValidTarget(parameters);
    }

    public virtual bool IsZoneValid(Zone zone, Player zoneOwner)
    {
      var p = new ZoneValidatorDelegateParameters(
        zone: zone,
        zoneOwner: zoneOwner,
        controller: _controller,
        game: Game,
        owningCard: _owningCard);

      return _isValidZone(p);
    }

    private bool CanBeTargeted(ITarget target, Card owningCard)
    {
      if (!_mustBeTargetable)
        return true;

      if (!target.IsCard())
        return true;

      return target.Card().CanBeTargetBySpellsOwnedBy(_controller) &&
        (owningCard == null || !target.Card().HasProtectionFrom(owningCard));
    }

    public virtual string GetMessage(int targetNumber, int? x)
    {
      var messageFormat = Message;

      if (messageFormat == null)
      {
        messageFormat = MaxCount != null && MaxCount.GetValue(x) == 1
          ? DefaultMessageOneTarget
          : DefaultMessageMultipleTargets;
      }

      if (_owningCard != null)
      {
        messageFormat = String.Format("{0}: {1}", _owningCard, messageFormat);
      }

      if (MaxCount != null)
      {
        var maxNumber = MinCount.GetValue(x) == MaxCount.GetValue(x)
          ? MaxCount.GetValue(x).ToString()
          : "max. " + MaxCount.GetValue(x);

        return string.Format(messageFormat, targetNumber, maxNumber);
      }

      return string.Format(messageFormat, targetNumber);
    }

    public bool HasValidZone(ITarget target)
    {
      var zone = target.Zone();

      if (zone == null)
        return true;

      var p = new ZoneValidatorDelegateParameters(
        zone: zone.Value,
        zoneOwner: target.Controller(),
        controller: _controller,
        game: Game,
        owningCard: _owningCard);

      return _isValidZone(p);
    }
  }
}