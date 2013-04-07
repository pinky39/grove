namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class EachPlayerReturnsCardFromGraveyardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private void ChooseCreatureToPutIntoPlay(Player player)
    {
      Enqueue<SelectCards>(
        controller: player,
        init: p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = FormatText("Select a creature card in your graveyard");
            p.Validator = card => card.Is().Creature;
            p.Zone = Zone.Graveyard;
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