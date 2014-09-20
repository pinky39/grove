namespace Grove.Effects
{
  using Modifiers;

  public class FlipACoinOpponentGainsOwningCard : Effect
  {
    private Player _opponent;

    protected override void Initialize()
    {
      // opponent is evaluated when this goes on stack
      _opponent = Controller.Opponent;
    }

    protected override void ResolveEffect()
    {
      var hasWon = FlipACoin(Controller);

      if (hasWon)
        return;

      if (Source.OwningCard.Zone != Zone.Battlefield)
        return;

      if (Source.OwningCard.Controller == _opponent)
        return;

      var sourceModifier = new ChangeController(_opponent);

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      Source.OwningCard.AddModifier(sourceModifier, p);
    }
  }
}