namespace Grove.Gameplay.Effects
{
  public class ExchangeCardsInBattlefieldAndGraveyard : Effect
  {
    public ExchangeCardsInBattlefieldAndGraveyard()
    {
      AllTargetsMustBeValid = true;
    }

    protected override void ResolveEffect()
    {
      var permanent = Targets.Effect[0].Card();
      var cardInGraveyard = Targets.Effect[1].Card();

      permanent.Sacrifice();
      cardInGraveyard.PutToBattlefield();
    }
  }
}