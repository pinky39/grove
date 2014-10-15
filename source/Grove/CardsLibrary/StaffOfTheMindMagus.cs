namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class StaffOfTheMindMagus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Staff of the Mind Magus")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever you cast a blue spell or an Island enters the battlefield under your control, you gain 1 life.")
        .FlavorText("A symbol of sagacity in bewildering times.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever you cast a blue spell or an Island enters the battlefield under your control, you gain 1 life.";

            p.Trigger(new OnCastedSpell((a, c) =>
              c.HasColor(CardColor.Blue) && c.Controller == a.OwningCard.Controller));

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (c, a, _) =>
                c.Is("Island") && c.Controller == a.OwningCard.Controller));

            p.Effect = () => new ChangeLife(amount: 1, forYou: true);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}