namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Targeting;
  using Zones;

  public class PutTargetsToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly bool _mustSacCreatureOnResolve;
    private readonly bool _tapped;

    private PutTargetsToBattlefield() {}

    public PutTargetsToBattlefield(bool mustSacCreatureOnResolve = false, bool tapped = false)
    {
      _mustSacCreatureOnResolve = mustSacCreatureOnResolve;
      _tapped = tapped;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => x.Score)
        .Take(1)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Sacrifice();
      }

      PutValidTargetsToBattlefield();
    }

    protected override void ResolveEffect()
    {
      if (_mustSacCreatureOnResolve)
      {
        if (Controller.Battlefield.Creatures.Count() == 0)
          return;

        SacCreatureAndPutValidTargetsToBattlefield();
        return;
      }

      PutValidTargetsToBattlefield();
    }

    private void SacCreatureAndPutValidTargetsToBattlefield()
    {
      Enqueue<SelectCards>(Controller, p =>
        {
          p.MinCount = 1;
          p.MaxCount = 1;
          p.Validator(card => card.Is().Creature);
          p.Zone = Zone.Battlefield;
          p.Text = FormatText("Select a creature to sacrifice");
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.OwningCard = Source.OwningCard;
        });
    }

    private void PutValidTargetsToBattlefield()
    {
      foreach (var target in ValidEffectTargets)
      {
        var card = target.Card();

        Controller.PutCardToBattlefield(card);

        if (_tapped)
        {
          card.Tap();
        }
      }
    }
  }
}