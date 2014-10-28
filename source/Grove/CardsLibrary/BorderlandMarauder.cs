namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class BorderlandMarauder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Borderland Marauder")
          .ManaCost("{1}{R}")
          .Type("Creature — Human Warrior")
          .Text("Whenever Borderland Marauder attacks, it gets +2/+0 until end of turn.")
          .FlavorText("Though she is rightly feared, there are relatively few tales of her deeds in battle, for few survive her raids.")
          .Power(1)
          .Toughness(2)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever Borderland Marauder attacks, it gets +2/+0 until end of turn.";

            p.Trigger(new WhenThisAttacks());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(2, 0) { UntilEot = true }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
          });
    }
  }
}
