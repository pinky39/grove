namespace Grove.Core.Effects
{
  using System;
  using Decisions;
  using Decisions.Results;
  using Infrastructure;
  using Zones;

  public class SacrificePermanentOrSacrificeOwner : Effect, IProcessDecisionResults<ChosenCards>
  {
    private readonly Func<Player, Card, bool> _shouldPayAi;
    private readonly string _text;
    private readonly Func<Card, bool> _validator;
    
    public SacrificePermanentOrSacrificeOwner(Func<Card, bool> validator = null, Func<Player, Card, bool> shouldPayAi = null, string text = null)
    {
      _shouldPayAi = shouldPayAi ?? delegate { return true; };
      _text = text ??  "Select permanent to sacrifice";
      _validator = validator ?? delegate { return true; };
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.None())
      {
        Source.OwningCard.Sacrifice();
      }
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsToSacrificeAsCost>(Controller, p =>
        {
          p.Ai = _shouldPayAi;
          p.QuestionText = FormatText("Pay upkeep cost?");
          p.Validator = _validator;
          p.Zone = Zone.Battlefield;
          p.MinCount = 1;
          p.MaxCount = 1;
          p.CardToPayUpkeepFor = Source.OwningCard;
          p.Text = _text;
          p.ProcessDecisionResults = this;
        });
    }
  }
}