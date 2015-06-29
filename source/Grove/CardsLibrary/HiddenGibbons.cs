namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HiddenGibbons : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Gibbons")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts an instant spell, if Hidden Gibbons is an enchantment, Hidden Gibbons becomes a 4/4 Ape creature.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts an instant spell, if Hidden Gibbons is an enchantment, Hidden Gibbons becomes a 4/4 Ape creature.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Instant));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 4,
                toughness: 4,
                type: t => t.Change(baseTypes: "creature", subTypes: "ape"),
                colors: L(CardColor.Green)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}