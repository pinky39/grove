namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class RaidersSpoils : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Raiders' Spoils")
        .ManaCost("{3}{B}")
        .Type("Enchantment")
        .Text("Creatures you control get +1/+0.{EOL}Whenever a Warrior you control deals combat damage to a player, you may pay 1 life. If you do, draw a card.")
        .FlavorText("\"To conquer is to eat.\"{EOL}—Edicts of Ilagra")
        .Cast(p =>
        {
          p.TimingRule(new OnFirstMain());
          p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower);
        })
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 0);
          p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever a Warrior you control deals combat damage to a player, you may pay 1 life. If you do, draw a card.";

          p.Trigger(new OnDamageDealt(dmg =>
            dmg.IsDealtToPlayer &&
            dmg.IsCombat &&
            dmg.Source.Is("warrior")));

          p.Effect = () => new PayLifeToDrawCard(1);

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
