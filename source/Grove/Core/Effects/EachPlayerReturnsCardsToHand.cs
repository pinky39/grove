namespace Grove.Core.Effects
{
  using System;
  using Decisions;
  using Zones;

  public class EachPlayerReturnsCardsToHand : Effect
  {
    private readonly bool _aiOrdersByDescendingScore;
    private readonly Func<Card, bool> _filter;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly string _text;
    private readonly Zone _zone;

    private EachPlayerReturnsCardsToHand() {}

    public EachPlayerReturnsCardsToHand(int minCount, int maxCount, Zone zone,
      bool aiOrdersByDescendingScore, string text, Func<Card, bool> filter = null)
    {
      _minCount = minCount;
      _text = text;
      _aiOrdersByDescendingScore = aiOrdersByDescendingScore;
      _zone = zone;
      _filter = filter ?? delegate { return true; };
      _maxCount = maxCount;
    }

    protected override void ResolveEffect()
    {
      ReturnCardToHand(Players.Active);
      ReturnCardToHand(Players.Passive);
    }

    private void ReturnCardToHand(Player player)
    {
      Enqueue<SelectCardsPutToHand>(
        controller: player,
        init: p =>
          {
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Validator = _filter;
            p.Zone = _zone;
            p.AiOrdersByDescendingScore = _aiOrdersByDescendingScore;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
          });
    }
  }
}