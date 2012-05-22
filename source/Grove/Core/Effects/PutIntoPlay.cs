namespace Grove.Core.Effects
{
  using System;

  public class PutIntoPlay : Effect
  {
    public Func<Player, bool> PutIntoPlayTapped = delegate { return false; };

    public override void Resolve()
    {
      var putIntoPlayTapped = PutIntoPlayTapped(Controller);
      Controller.PutCardIntoPlay(Source.OwningCard);

      if (putIntoPlayTapped)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}