namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class ApplyActionToPermanentOrApplyActionToOwner : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly string _instructions;
    private readonly Func<Player, Card, bool> _shouldPayAi;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    private readonly Action<Player, Card> _actionToPermament;
    private readonly Action<Player, Card> _actionToOwner;
    private readonly bool _canSelectSelf;

    private ApplyActionToPermanentOrApplyActionToOwner() {}

    public ApplyActionToPermanentOrApplyActionToOwner(Func<Card, bool> validator = null, Action<Player, Card> actionToPermament = null,
      Action<Player, Card> actionToOwner = null, bool canSelectSelf = true,
      Func<Player, Card, bool> shouldPayAi = null, string text = null, string instructions = null)
    {
      _instructions = instructions;
      _shouldPayAi = shouldPayAi ?? delegate { return true; };
      _text = text ?? "Select permanent to sacrifice.";
      _validator = validator ?? delegate { return true; };
      _actionToPermament = actionToPermament ?? delegate { };
      _actionToOwner = actionToOwner ?? delegate { };
      _canSelectSelf = canSelectSelf;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      if (_shouldPayAi(Controller, Source.OwningCard))
      {
        return candidates
          .Where(x => _canSelectSelf || x != Source.OwningCard)
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
        _actionToOwner(Controller, Source.OwningCard);
        return;
      }

      _actionToPermament(results[0].Controller, results[0]);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller, p =>
        {
          p.SetValidator(_validator);
          p.Zone = Zone.Battlefield;
          p.MinCount = 0;
          p.MaxCount = 1;
          p.Text = _text;
          p.Instructions = _instructions;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.OwningCard = Source.OwningCard;
        }));
    }
  }
}