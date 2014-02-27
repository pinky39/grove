namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;

  public class DrawCardsEqualToSacrificedPermanentsCount : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    private DrawCardsEqualToSacrificedPermanentsCount() {}

    public DrawCardsEqualToSacrificedPermanentsCount(string text, Func<Card, bool> validator = null)
    {
      _text = text;
      _validator = validator ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      // simle ai rule
      // sacrifice lands until count = 6
      // sacrifice creatures with power < 3
      var result = new List<Card>();

      var lands = candidates.Where(x => x.Is().Land)
        .OrderBy(x => x.Score)
        .ToList();

      var creatures = candidates
        .Where(x => x.Is().Creature && x.Power < 3 && !x.Has().AnyEvadingAbility)
        .OrderBy(x => x.Score)
        .ToList();

      if (lands.Count > 6)
      {
        result.AddRange(lands.Take(lands.Count - 6));
      }

      result.AddRange(creatures);

      return result;
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Sacrifice();
      }

      Controller.DrawCards(results.Count);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller,
        p =>
          {
            p.SetValidator(_validator);
            p.Zone = Zone.Battlefield;
            p.MinCount = 0;
            p.MaxCount = null;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        ));
    }
  }
}