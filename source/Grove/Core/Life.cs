namespace Grove.Core
{
  using Ai;
  using Infrastructure;

  [Copyable]
  public class Life
  {
    private readonly Trackable<int> _score;
    private readonly Trackable<int> _value;

    public Life(int value, ChangeTracker changeTracker)
    {
      _value = new Trackable<int>(value, changeTracker);
      _score = new Trackable<int>(ScoreCalculator.CalculateLifeScore(value), changeTracker);
    }

    private Life() {}

    public int Score { get { return _score.Value; } private set { _score.Value = value; } }

    public int Value
    {
      get { return _value.Value; }
      set
      {
        if (value == _value.Value)
          return;

        _value.Value = value;
        Score = ScoreCalculator.CalculateLifeScore(value);
      }
    }
  }
}