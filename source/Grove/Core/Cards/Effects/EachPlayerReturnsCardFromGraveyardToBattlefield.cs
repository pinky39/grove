namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Decisions;
  using Grove.Core.Zones;

  public class EachPlayerReturnsCardFromGraveyardToBattlefield : Effect
  {
    private void ChooseCreatureToPutIntoPlay(Player player)
    {
      Game.Enqueue<SelectCardsPutToBattlefield>(
        controller: player,
        init: p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Text = FormatText("Select a creature card in your graveyard");
            p.Validator = card => card.Zone == Zone.Graveyard && card.Is().Creature;            
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