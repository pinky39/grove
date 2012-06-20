namespace Grove.Core.Effects
{
  public class PayManaOrSacrifice : Effect
  {
    public IManaAmount Amount { get; set; }

    protected override void ResolveEffect()
    {
      if (Controller.HasMana(Amount) == false)
      {
        Source.OwningCard.Sacrifice();
        return;
      }

      Decisions.EnqueueConsiderPayingLifeOrMana(
        player: Controller,
        ctx: this,
        mana: Amount,
        handler: args =>
          {
            var effect = args.Ctx<Effect>();
            if (args.Answer)
            {
              effect.Controller.Consume(Amount);
              return;
            }

            effect.Source.OwningCard.Sacrifice();
          });
    }
  }
}