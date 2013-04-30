namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Card;
  using Decisions;
  using Decisions.Results;
  using Modifiers;
  using Zones;

  public class PutSelectedCardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    private readonly Zone _zone;

    private PutSelectedCardToBattlefield() {}

    public PutSelectedCardToBattlefield(string text, Func<Card, bool> validator, Zone zone,
      params ModifierFactory[] modifiers)
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
              Target = card,
              X = X
            };

          var modifier = modifierFactory().Initialize(p, Game);
          card.AddModifier(modifier);
        }
      }
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
  }
}