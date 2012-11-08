namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;


  public class EachPlayerReturnsCardFromGraveyardToBattlefield : Effect
  {
    private void ChooseCreatureToPutIntoPlay(Player player)
    {
      Game.Enqueue<ReturnCardsFromGraveyardToBattlefield>(
        controller: player,
        init: p =>
          {
            p.Count = 1;
            p.Text = "Select a creature in your graveyard";
            p.Filter = card => card.Is().Creature;
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