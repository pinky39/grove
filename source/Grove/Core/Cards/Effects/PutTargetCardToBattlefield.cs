namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using Controllers;

  public class PutTargetCardToBattlefield : Effect
  {
    public string Text;
    public Func<Card, bool> Validator;

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsPutToBattlefield>(Controller,
        p =>
          {
            p.Validator = Validator;
            p.MinCount = 0;
            p.MaxCount = 1;
            p.Text = FormatText(Text);
          }
        );
    }
  }
}