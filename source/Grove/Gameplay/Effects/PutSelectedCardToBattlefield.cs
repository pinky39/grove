namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class PutSelectedCardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
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
      Enqueue<SelectCards>(Controller,
        p =>
          {
            p.Validator(_validator);
            p.Zone = _zone;
            p.MinCount = 0;
            p.MaxCount = 1;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        );
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToBattlefield();
      }
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(1)
        .ToList();
    }    
  }
}