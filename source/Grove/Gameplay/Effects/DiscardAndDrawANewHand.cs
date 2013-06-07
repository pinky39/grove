namespace Grove.Gameplay.Effects
{
  public class DiscardAndDrawANewHand : Effect
  {
    protected override void ResolveEffect()
    {
      var count = Controller.Hand.Count;
      Controller.DiscardHand();
      Controller.DrawCards(count);
    }
  }
}