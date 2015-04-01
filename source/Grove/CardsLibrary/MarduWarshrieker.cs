namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class MarduWarshrieker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Warshrieker")
        .ManaCost("{3}{R}")
        .Type("Creature — Orc Shaman")
        .Text("{I}Raid{/I} — When Mardu Warshrieker enters the battlefield, if you attacked with a creature this turn, add {R}{W}{B} to your mana pool.")
        .FlavorText("\"No body can contain so much fury. It reminds me of another battle, long past.\"{EOL}—Sarkhan Vol")
        .Power(3)
        .Toughness(3)        
        .TriggeredAbility(p =>
        {
          p.Text = "When Mardu Warshrieker enters the battlefield, if you attacked with a creature this turn, add {R}{W}{B} to your mana pool.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Turn.Events.HasActivePlayerAttackedThisTurn,
          });

          p.Effect = () => new AddManaToPool("{R}{W}{B}".Parse());
        });
    }
  }
}
