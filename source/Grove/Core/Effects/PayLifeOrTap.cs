namespace Grove.Core.Effects
{
  public class PayLifeOrTap : Effect
  {
    public int Life { get; set; }

    public override void Resolve()
    {
      Decisions.EnqueueConsiderPayingLifeOrMana(
        player: Controller,
        effect: this,
        life: Life,
        handler: args => {
          if (args.Answer)
          {
            args.Effect.Controller.Life -= Life;
            return;
          }

          args.Effect.Source.OwningCard.Tap();
        });
    }
  }
}