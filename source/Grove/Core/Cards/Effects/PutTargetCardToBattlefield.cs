namespace Grove.Core.Cards.Effects
{
  using System;
  using Decisions;
  using Zones;

  public class PutTargetCardToBattlefield : Effect
  {
    public string Text;
    public Func<Card, bool> Validator;
    public Func<Zone, bool> Zone;

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsPutToBattlefield>(Controller,
        p =>
          {
            p.Validator = Validator;
            p.Zone = Zone;
            p.MinCount = 0;
            p.MaxCount = 1;
            p.Text = FormatText(Text);
          }
        );
    }
  }
}