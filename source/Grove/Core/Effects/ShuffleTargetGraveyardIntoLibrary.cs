namespace Grove.Effects
{
  using System;
  using System.Linq;

  public class ShuffleTargetGraveyardIntoLibrary : Effect
  {
    private readonly Func<Card, bool> _selector;

    private ShuffleTargetGraveyardIntoLibrary() {}

    public ShuffleTargetGraveyardIntoLibrary(Func<Card, bool> selector)
    {
      _selector = selector;
    }

    protected override void ResolveEffect()
    {
      var player = Target.Player();
      var cards = player.Graveyard.Where(_selector).ToList();
      player.ShuffleIntoLibrary(cards);
    }
  }
}