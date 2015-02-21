namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;

  public class BloodsoakedChampion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodsoaked Champion")
        .ManaCost("{B}")
        .Type("Creature — Human Warrior")
        .Text("Bloodsoaked Champion can't block.{EOL}{I}Raid{/I} — {1}{B}: Return Bloodsoaked Champion from your graveyard to the battlefield. Activate this ability only if you attacked with a creature this turn.")
        .FlavorText("\"Death is merely another foe the Mardu will overcome.\"")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{B}: Return Bloodsoaked Champion from your graveyard to the battlefield.";

          p.Cost = new PayMana("{1}{B}".Parse());
          p.Condition = (card, game) => game.Turn.Events.HasActivePlayerAttackedThisTurn;
          p.Effect = () => new PutOwnerToBattlefield(from: Zone.Graveyard);
          p.ActivationZone = Zone.Graveyard;
        });
    }
  }
}
