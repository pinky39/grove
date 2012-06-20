namespace Grove.Core.Effects
{
  public class PayLifeOrTap : Effect
  {
    public int Life { get; set; }

    protected override void ResolveEffect()
    {
      Decisions.EnqueueConsiderPayingLifeOrMana(
        player: Controller,
        ctx: this,
        life: Life,
        handler: args => {
          
          var effect = args.Ctx<Effect>();
          
          if (args.Answer)
          {            
            effect.Controller.Life -= Life;
            return;
          }

          effect.Source.OwningCard.Tap();
        });
    }
  }
}