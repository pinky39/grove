namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class EachPlayerReturnsCardFromZoneToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _selector;
    private readonly Zone _zone;

    private EachPlayerReturnsCardFromZoneToBattlefield() {}

    public EachPlayerReturnsCardFromZoneToBattlefield(Zone zone, Func<Card, bool> selector = null)
    {
      _zone = zone;
      _selector = selector ?? delegate { return true; };
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
      }
    }

    private void ChooseCreatureToPutIntoPlay(Player player)
    {
      Enqueue<SelectCards>(
        controller: player,
        init: p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = "Select a card.";
            p.Validator(_selector);
            p.Zone = _zone;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        );
    }

    protected override void ResolveEffect()
    {
      ChooseCreatureToPutIntoPlay(Players.Active);
      ChooseCreatureToPutIntoPlay(Players.Passive);
    }
  }
}