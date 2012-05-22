namespace Grove.Core.Effects
{
  public class PayManaOrSacrifice : Effect
  {
    public ManaAmount Amount { get; set; }
    
    public override void Resolve()
    {
      
      if (Controller.HasEnoughMana(Amount) == false)
      {
        Source.OwningCard.Sacrifice();
        return;
      }

      Decisions.EnqueueConsiderPayingLifeOrMana(
        player: Controller,
        effect: this,
        mana: Amount,
        handler: args => {
          if (args.Answer)
          {
            args.Effect.Controller.Consume(Amount);
            return;
          }

          args.Effect.Source.OwningCard.Sacrifice();
        });            
    }
  }
}