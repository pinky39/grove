namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class IlluminatedWings : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Illuminated Wings")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature has flying.{EOL}{2}, Sacrifice Illuminated Wings: Draw a card.")
        .FlavorText("For a moment, Urza thought not of war and destruction, but of the freedom of the skies.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddStaticAbility(Static.Flying));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment(filter: x => !x.Has().Flying));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}, Sacrifice Illuminated Wings: Draw a card.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DrawCards(1);

            p.TimingRule(new Any(
              new WhenAttachedToCardWillBeDestroyed(),
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));

            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}