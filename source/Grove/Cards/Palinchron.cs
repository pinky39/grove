namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Palinchron : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Palinchron")
        .ManaCost("{5}{U}{U}")
        .Type("Creature Illusion")
        .Text("{Flying}{EOL}When Palinchron enters the battlefield, untap up to seven lands.{EOL}{2}{U}{U}: Return Palinchron to its owner's hand.")        
        .Power(4)
        .Toughness(5)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Palinchron enters the battlefield, untap up to seven lands.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 7,
              validator: c => c.Is().Land,
              text: "Select lands to untap."
              );
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{U}{U}: Return Palinchron to its owner's hand.";
            p.Cost = new PayMana("{2}{U}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new Gameplay.Effects.ReturnToHand(returnOwningCard: true);

            p.TimingRule(new WhenOwningCardWillBeDestroyed());
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}