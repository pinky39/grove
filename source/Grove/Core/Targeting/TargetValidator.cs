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
    private string _textFormat = "Select a target.";
    private TargetValidatorDelegate _validator = delegate { return true; };
    public Player Controller { get { return Source.Controller; } }
    public bool MustBeTargetable { get { return _mustBeTargetable; } set { _mustBeTargetable = value; } }
    public Card Source { get; private set; }
    public string TextFormat { get { return _textFormat; } set { _textFormat = value; } }
    public TargetValidatorDelegate Validator { get { return _validator; } set { _validator = value; } }
    public int? MaxCount { get { return _maxCount; } set { _maxCount = value; } }
    public int MinCount { get { return _minCount; } set { _minCount = value; } }
    public string Text { get { return string.Format(TextFormat, MinCount, MaxCount); } }

    public bool IsValid(ITarget target)
    {
      var valid = Validator(new TargetValidatorParameters(target, Source, _game));

      if (target.IsCard())
        valid = valid && IsTargetable(target.Card());

      return valid;
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

        Init(targetSelector);

        return targetSelector;
      }
    }
  }
}