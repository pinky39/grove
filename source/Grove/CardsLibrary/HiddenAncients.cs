namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HiddenAncients : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Ancients")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.")
        .FlavorText("The only alert the invaders had was the rustling of leaves on a day without wind.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.";

            p.Trigger(new OnCastedSpell((c, ctx) => 
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToCreature(
              power: 5,
              toughness: 5,
              type: t => t.Change(baseTypes: "creature", subTypes: "treefolk"),
              colors: L(CardColor.Green)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}