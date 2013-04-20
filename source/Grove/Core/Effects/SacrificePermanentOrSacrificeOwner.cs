namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Infrastructure;
  using Zones;

  public class SacrificePermanentOrSacrificeOwner : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Player, Card, bool> _shouldPayAi;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    private SacrificePermanentOrSacrificeOwner() {}

    public SacrificePermanentOrSacrificeOwner(Func<Card, bool> validator = null,
      Func<Player, Card, bool> shouldPayAi = null, string text = null)
    {
      _shouldPayAi = shouldPayAi ?? delegate { return true; };
      _text = text ?? "Select permanent to sacrifice";
      _validator = validator ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      if (_shouldPayAi(Controller, Source.OwningCard))
      {
        return candidates
          .OrderBy(x => x.Score)
          .Take(1)
          .ToList();
      }

      return new ChosenCards();
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.None())
      {
        Source.OwningCard.Sacrifice();
        return;
      }

      results[0].Sacrifice();
    }

    protected override void ResolveEffect()
    {
      Enqueue<SelectCards>(Controller, p =>
        {
          p.Validator(_validator);
          p.Zone = Zone.Battlefield;
          p.MinCount = 0;
          p.MaxCount = 1;
          p.Text = _text;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.OwningCard = Source.OwningCard;
        });
    }    
  }
}