namespace Grove.Gameplay
{
  public class Aura : Sorcery
  {
    public override void AfterResolve()
    {
      var attachedCardController = GetController(Card);
      attachedCardController.PutCardToBattlefield(Card);
    }

    private Player GetController(Card card)
    {
      if (card.AttachedTo == null)
        return card.Controller;

      return GetController(card.AttachedTo);
    }
  }
}