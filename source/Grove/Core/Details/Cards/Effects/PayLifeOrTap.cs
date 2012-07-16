namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class PayLifeOrTap : Effect
  {
    public int Life { get; set; }

    protected override void ResolveEffect()
    {
      Decisions.Enqueue<ConsiderPayingLifeOrMana>(
        controller: Controller,
        init: p =>
          {
            p.Context = this;
            p.Life = Life;
            p.Handler = args =>
              {
                var effect = args.Ctx<Effect>();
                
                if (args.Answer)
                {
                  effect.Controller.Life -= Life;
                  return;
                }

                effect.Source.OwningCard.Tap();
              };
          });
    }
  }
}