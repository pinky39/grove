namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using Grove.AI;
  using Grove.Decisions;

  public class EachPlayerPutsCardToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _selector;
    private readonly Zone _zone;

    private EachPlayerPutsCardToBattlefield() {}

    public EachPlayerPutsCardToBattlefield(Zone zone, Func<Card, bool> filter = null)
    {
      _zone = zone;
      _selector = filter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return CardPicker.ChooseBestCards(candidates, count: 1, aurasNeedTarget: true);
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 0)
       return;

      if (results.Count == 1)
      {
        results[0].PutToBattlefield();
        return;
      }

      results[0].EnchantWithoutPayingCost(results[1]);      
    }

    private void ChooseCardToPutIntoPlay(Player player)
    {
      Enqueue(new SelectCards(
        player,
        p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = "Select a card.";
            p.SetValidator(_selector);
            p.Zone = _zone;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.AurasNeedTarget = true;
          }
        ));
    }

    protected override void ResolveEffect()
    {
      ChooseCardToPutIntoPlay(Players.Active);
      ChooseCardToPutIntoPlay(Players.Passive);
    }
  }
}