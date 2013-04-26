namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class ManOWar : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Man-o'-War")
        .ManaCost("{2}{U}")
        .Type("Creature Jellyfish")
        .Text("When Man-o'-War enters the battlefield, return target creature to its owner's hand.")
        .FlavorText("Beauty to the eye does not always translate to the touch.")
        .Power(2)
        .Toughness(2)
        .Cast(p =>
          {
            p.TimingRule(new OpponentHasPermanents(
              card => card.Is().Creature && card.CanBeTargetBySpellsWithColor(CardColor.Blue)));
            p.TimingRule(new FirstMain());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Man-o'-War enters the battlefield, return target creature to its owner's hand.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ReturnToHand {Category = EffectCategories.Bounce};
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new Bounce());
          });
    }
  }
}