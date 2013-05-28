namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  [Serializable]
  public class UntapSelectedPermanents : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    private UntapSelectedPermanents() {}

    public UntapSelectedPermanents(int minCount, int maxCount, Func<Card, bool> validator = null, string text = null)
    {
      _minCount = minCount;
      _validator = validator ?? delegate { return true; };
      _text = text ?? "Select permanents to untap.";
      _maxCount = maxCount;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(_maxCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Untap();
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue<SelectCards>(Controller,
        p =>
          {
            p.Validator(_validator);
            p.Zone = Zone.Battlefield;
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        );
    }
  }
}