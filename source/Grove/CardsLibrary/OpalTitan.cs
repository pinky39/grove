namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class OpalTitan : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Titan")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
                ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 4,
                toughness: 4,
                type: t => t.Change(baseTypes: "creature", subTypes: "giant"),
                colors: L(CardColor.White)),
              () => new AddProtectionFromColors(m =>
                m.SourceEffect.TriggerMessage<SpellPutOnStackEvent>().Card.Colors));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}