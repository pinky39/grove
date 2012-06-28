namespace Grove.Core
{
  using System;
  using Infrastructure;

  public delegate int ScoreCalculator(ITarget target, Card source, int? maxX, Game game);
  public delegate bool TargetValidatorDelegate(ITarget target, Card source, Game game);

  public interface ITargetSelectorFactory
  {
    TargetSelector Create(Card source);
  }

  public interface ITargetSelector
  {
    int? MaxCount { get; }
    int MinCount { get; }
    string Text { get; }
    bool IsValid(ITarget target);
  }

  public static class Validator
  {
    public static Func<ITarget, Card, bool> Equipment()
    {
      return (target, equipment) => {
        if (!target.Is().Creature) return false;

        if (target.Card().Controller != equipment.Controller)
          return false;

        return !equipment.IsAttached || equipment.AttachedTo != target;
      };
    } 
  }

  [Copyable]
  public class TargetSelector : ITargetSelector
  {    
    private Game _game;
    private int? _maxCount = 1;
    private int _minCount = 1;
    private bool _mustBeTargetable = true;
    private ScoreCalculator _scorer = delegate { return WellKnownTargetScores.Neutral; };
    private string _textFormat = "Select {0} target(s).";
    private TargetValidatorDelegate _validator = delegate { return true; };    
    public Player Controller { get { return Source.Controller; } }
    public int? MaxCount { get { return _maxCount; } set { _maxCount = value; } }
    public int MinCount { get { return _minCount; } set { _minCount = value; } }
    public bool MustBeTargetable { get { return _mustBeTargetable; } set { _mustBeTargetable = value; } }    
    public Card Source { get; private set; }
    public string Text { get { return string.Format(TextFormat, MinCount, MaxCount); } }
    public string TextFormat { get { return _textFormat; } set { _textFormat = value; } }
    public TargetValidatorDelegate Validator { get { return _validator; } set { _validator = value; } }    

    public bool IsValid(ITarget target)
    {
      var valid = Validator(target, Source, _game);

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
    
    public class Factory : ITargetSelectorFactory
    {
      public Game Game { get; set; }
      public Action<TargetSelector> Init { get; set; }

      public TargetSelector Create(Card source)
      {
        var targetSelector = new TargetSelector();
        targetSelector._game = Game;
        targetSelector.Source = source;
        targetSelector.MustBeTargetable = true;

        Init(targetSelector);

        return targetSelector;
      }
    }
  }
}