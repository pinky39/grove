namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;
  using Modifiers;

  public class PutSelectedCardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    private readonly Zone _zone;

    private PutSelectedCardToBattlefield() {}

    public PutSelectedCardToBattlefield(string text, Func<Card, bool> validator, Zone zone,
      params CardModifierFactory[] modifiers)
    {
      _text = text;
      _zone = zone;
      _validator = validator;
      _modifiers.AddRange(modifiers);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(1)
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
            p.MaxCount = 1;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        ));
    }
  }
}