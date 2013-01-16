namespace Grove.Core.Targeting
{
  using System;
  using Core.Zones;
  using Infrastructure;

  public delegate bool TargetValidatorDelegate(TargetValidatorParameters parameters);

  public delegate bool ZoneValidatorDelegate(ZoneValidatorParameters parameters);

  [Copyable]
  public class TargetValidator : ITargetValidator
  {
    private const string DefaultMessageOneTarget = "Select a target.";
    private const string DefaultMessageMultipleTargets = "Select target: {0} of {1}.";
    public TargetValidatorDelegate Target;
    public ZoneValidatorDelegate Zone;
    private Game _game;
    private int? _maxCount = 1;
    private int _minCount = 1;
    private bool _mustBeTargetable = true;
    private Trackable<object> _trigger;
    public Player Controller { get { return Source.Controller; } }
    public bool MustBeTargetable { get { return _mustBeTargetable; } set { _mustBeTargetable = value; } }
    public Card Source { get; private set; }
    public string MessageFormat { get; set; }

    public object Trigger { get { return _trigger.Value; } set { _trigger.Value = value; } }
    public int? MaxCount { get { return _maxCount; } set { _maxCount = value; } }
    public int MinCount { get { return _minCount; } set { _minCount = value; } }

    public bool IsValid(ITarget target)
    {
      if (target.IsCard() && !IsTargetable(target.Card()))
      {
        return false;
      }

      var parameters = new TargetValidatorParameters(
        target,
        Source,
        Trigger,
        _game
        );

      return Target(parameters);
    }

    public string GetMessage(int targetNumber)
    {
      var messageFormat =
        MessageFormat ??
          (_maxCount == 1 ? DefaultMessageOneTarget : DefaultMessageMultipleTargets);

      var maxNumber = MinCount == MaxCount ? MaxCount.ToString() : "max. " + MaxCount;

      return
        string.Format("{0}: ", Source) +
          string.Format(messageFormat, targetNumber, maxNumber);
    }

    public bool IsValidZone(Zone zone, Player owner)
    {
      var parameters = new ZoneValidatorParameters(zone, owner, Source, _game);
      return Zone(parameters);
    }

    private bool IsTargetable(Card target)
    {
      if (!MustBeTargetable)
        return true;

      return target.CanBeTargetBySpellsOwnedBy(Controller) &&
        !target.HasProtectionFrom(Source);
    }

    public class Factory : ITargetValidatorFactory
    {
      public Action<TargetValidator> Init { get; set; }

      public TargetValidator Create(Card source, Game game)
      {
        var targetSelector = new TargetValidator();
        targetSelector._game = game;
        targetSelector.Source = source;
        targetSelector.MustBeTargetable = true;
        targetSelector._trigger = new Trackable<object>(game.ChangeTracker);

        Init(targetSelector);


        return targetSelector;
      }
    }
  }
}