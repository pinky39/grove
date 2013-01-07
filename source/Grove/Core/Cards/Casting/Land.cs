namespace Grove.Core.Cards.Casting
{
  using Effects;

  public class Land : CastingRule
  {
    public override bool CanCast()
    {
      return Game.Turn.Step.IsMain() &&
        Card.Controller.IsActive &&
          Game.Stack.IsEmpty &&
            Card.Controller.CanPlayLands;
    }

    public override void Cast(Effect effect)
    {
      Card.Controller.LandsPlayedCount++;

      effect.Resolve();
      effect.FinishResolve();
    }

    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }
  }
}