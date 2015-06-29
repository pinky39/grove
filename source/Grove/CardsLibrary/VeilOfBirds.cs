namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class VeilOfBirds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veil of Birds")
        .ManaCost("{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veil of Birds is an enchantment, Veil of Birds becomes a 1/1 Bird creature with flying.")
        .FlavorText("When wind marries sky, even the bride's veil sings her praises.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veil of Birds is an enchantment, Veil of Birds becomes a 1/1 Bird creature with flying.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 1,
                toughness: 1,
                type: t => t.Change(baseTypes: "creature", subTypes: "bird"),
                colors: L(CardColor.Blue)),
              () => new AddStaticAbility(Static.Flying));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}