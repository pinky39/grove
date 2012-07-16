namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Mana;

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

      Decisions.Enqueue<ConsiderPayingLifeOrMana>(
        controller: Controller,
        init: p =>
          {
            p.Context = this;
            p.Mana = Amount;
            p.Handler = args =>
              {
                var effect = args.Ctx<Effect>();
                if (args.Answer)
                {
                  effect.Controller.Consume(Amount);
                  return;
                }

                effect.Source.OwningCard.Sacrifice();
              };
          });
    }
  }
}