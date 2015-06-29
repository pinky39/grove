namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class OpalGargoyle : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Gargoyle")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 2,
                toughness: 2,
                type: t => t.Change(baseTypes: "creature", subTypes: "gargoyle"),
                colors: L(CardColor.White)),
              () => new AddStaticAbility(Static.Flying));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}