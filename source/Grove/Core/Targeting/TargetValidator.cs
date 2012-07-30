namespace Grove.Core.Targeting
{
  using System;
  using Infrastructure;

  public delegate bool TargetValidatorDelegate(TargetValidatorParameters parameters);
  
  [Copyable]
  public class TargetValidator : ITargetValidator
  {
    private Game _game;
    private int? _maxCount = 1;
    private int _minCount = 1;
    private bool _mustBeTargetable = true;    
    private const string DefaultMessageOneTarget = "Select a target.";
    private const string DefaultMessageMultipleTargets = "Select target: {0} of {1}.";
    
    private TargetValidatorDelegate _validator = delegate { return true; };
    private Trackable<object> _trigger;
    public Player Controller { get { return Source.Controller; } }
    public bool MustBeTargetable { get { return _mustBeTargetable; } set { _mustBeTargetable = value; } }
    public Card Source { get; private set; }
    public string MessageFormat { get; set; }
    public TargetValidatorDelegate Validator { get { return _validator; } set { _validator = value; } }
    public int? MaxCount { get { return _maxCount; } set { _maxCount = value; } }
    public int MinCount { get { return _minCount; } set { _minCount = value; } }
    public object Trigger { get { return _trigger.Value; } set { _trigger.Value = value; } }
           
    public bool IsValid(ITarget target)
    {
      var parameters = new TargetValidatorParameters(
        target, 
        Source, 
        Trigger,
        _game
        );
      
      var valid = Validator(parameters);

      if (target.IsCard())
        valid = valid && IsTargetable(target.Card());

      return valid;
    }

    public string GetMessage(int targetNumber)
    {
      var messageFormat = 
        MessageFormat ??
        (_maxCount == 1 ? DefaultMessageOneTarget : DefaultMessageMultipleTargets);

      var maxNumber = MinCount == MaxCount ? MaxCount.ToString() : "max. " + MaxCount.ToString();

      return string.Format(messageFormat, targetNumber, maxNumber);           
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
      public Game Game { get; set; }
      public Action<TargetValidator> Init { get; set; }

      public TargetValidator Create(Card source)
      {
        var targetSelector = new TargetValidator();
        targetSelector._game = Game;
        targetSelector.Source = source;
        targetSelector.MustBeTargetable = true;        
        targetSelector._trigger = new Trackable<object>(Game.ChangeTracker);

        Init(targetSelector);
        

        return targetSelector;
      }
    }
  }
}