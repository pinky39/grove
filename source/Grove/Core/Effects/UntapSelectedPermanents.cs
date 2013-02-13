namespace Grove.Core.Effects
{
  using System;
  using Decisions;

  public class UntapSelectedPermanents : Effect
  {
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    public UntapSelectedPermanents(int minCount, int maxCount, Func<Card, bool> validator = null, string text = null)
    {
      _minCount = minCount;
      _validator = validator ?? delegate { return true; };
      _text = text ?? "Select permanents to untap.";
      _maxCount = maxCount;
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsToUntap>(Controller,
        p =>
          {
            p.Validator = _validator;
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Text = FormatText(_text);
          }
        );
    }
  }
}