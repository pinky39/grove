namespace Grove.Core.Details.Cards.Effects
{
  using System.Linq;
  using Controllers.Human;
  using Controllers.Results;
  using Infrastructure;
  using Targeting;
  using Ui;

  public class PutTargetsToBattlefield : Effect
  {
    public bool MustSacCreatureOnResolve;
    public bool Tapped;       

    protected override void ResolveEffect()
    {
      if (MustSacCreatureOnResolve)
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
      Decisions.Enqueue<Controllers.AdhocDecision<ChosenCards>>(
        controller: Controller,
        init: p =>
          {
            p.Param("effect", this);
            p.QueryAi = self =>
              {
                var creatureToSac =
                  self.Controller.Battlefield.Creatures
                    .OrderBy(x => x.Score);

                return new ChosenCards(creatureToSac);
              };
            p.QueryUi = self =>
              {
                var chosenCards = new ChosenCards();

                var dialog = self.TargetDialog.Create(
                  new UiTargetValidator(
                    minTargetCount: 0,
                    maxTargetCount: 1,
                    text: "Select a creature to sacrifice",
                    isValid: target => target.IsPermanent && target.Is().Creature &&
                      target.Controller == self.Controller),
                  canCancel: false
                  );

                self.Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);
                chosenCards.Add(dialog.Selection[0].Card());

                return chosenCards;
              };
            p.Process = self =>
              {
                if (self.Result.None())
                  return;

                var creatureToSac = self.Result.First();
                creatureToSac.Sacrifice();

                var effect = p.Param<PutTargetsToBattlefield>("effect");
                effect.PutValidTargetsToBattlefield();
              };
          });
    }

    private void PutValidTargetsToBattlefield()
    {
      foreach (var target in ValidTargets)
      {
        var card = target.Card();
        
        Controller.PutCardToBattlefield(card);
        
        if (Tapped)
        {
          card.Tap();
        }
      }
    }
  }
}