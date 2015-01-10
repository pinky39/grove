namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;
  using Modifiers;

  public class PutSelectedCardsToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    private readonly Zone _zone;
    private readonly Action<Card, Game> _afterCardPutToBattlefield;
    private readonly Value _count;

    private PutSelectedCardsToBattlefield() {}

    public PutSelectedCardsToBattlefield(string text, Func<Card, bool> validator, Zone zone,
      params CardModifierFactory[] modifiers) : this(text, validator, zone, null, 1, modifiers) {}

    public PutSelectedCardsToBattlefield(string text, Func<Card, bool> validator, Zone zone, Action<Card, Game> afterCardPutToBattlefield, Value count,
      params CardModifierFactory[] modifiers)
    {
      _text = text;
      _zone = zone;
      _count = count ?? 1;
      _validator = validator;
      _modifiers.AddRange(modifiers);
      _afterCardPutToBattlefield = afterCardPutToBattlefield ?? delegate { };
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

        _afterCardPutToBattlefield(card, Game);
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller,
        p =>
          {
            p.SetValidator(_validator);
            p.Zone = _zone;
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