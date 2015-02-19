namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Modifiers;

  public class PutSelectedCardsToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    private readonly Zone _fromZone;
    private readonly Action<Card, Game> _after;
    private readonly Value _count;

    private PutSelectedCardsToBattlefield() {}

    public PutSelectedCardsToBattlefield(
      Zone fromZone,
      Func<Card, bool> validator = null,
      string text = null,
      Action<Card, Game> after = null,
      Value count = null,
      IEnumerable<CardModifierFactory> modifiers = null)
    {
      _fromZone = fromZone;
      _text = text ?? "Select a card.";
      _count = count ?? 1;
      _validator = validator ?? delegate { return true; };

      if (modifiers != null)
      {
        _modifiers.AddRange(modifiers);
      }

      _after = after ?? delegate { };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(_count.GetValue(X))
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToBattlefield();

        foreach (var modifierFactory in _modifiers)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              X = X
            };

          var modifier = modifierFactory();
          card.AddModifier(modifier, p);
        }

        _after(card, Game);
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller,
        p =>
          {
            p.SetValidator(_validator);
            p.Zone = _fromZone;
            p.MinCount = 0;
            p.MaxCount = _count.GetValue(X);
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        ));
    }
  }
}