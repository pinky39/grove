namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class OpalArchangel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Archangel")
        .ManaCost("{4}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Archangel is an enchantment, Opal Archangel becomes a 5/5 Angel creature with flying and vigilance.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Archangel is an enchantment, Opal Archangel becomes a 5/5 Angel creature with flying and vigilance.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 5,
                toughness: 5,
                type: t => t.Change(baseTypes: "creature", subTypes: "angel"),
                colors: L(CardColor.White)),
              () => new AddSimpleAbility(Static.Flying),
              () => new AddSimpleAbility(Static.Vigilance));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}