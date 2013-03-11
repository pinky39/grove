namespace Grove.Core.Effects
{
  using System;
  using Decisions;
  using Zones;

  public class PutSelectedCardToBattlefield : Effect
  {
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    private readonly Zone _zone;

    private PutSelectedCardToBattlefield() {}

    public PutSelectedCardToBattlefield(string text, Func<Card, bool> validator, Zone zone)
    {
      _text = text;
      _zone = zone;
      _validator = validator;
    }

    protected override void ResolveEffect()
    {
      Enqueue<SelectCardsPutToBattlefield>(Controller,
        p =>
          {
            p.Validator = _validator;
            p.Zone = _zone;
            p.MinCount = 0;
            p.MaxCount = 1;
            p.Text = FormatText(_text);
            p.OwningCard = Source.OwningCard;
          }
        );
    }
  }
}