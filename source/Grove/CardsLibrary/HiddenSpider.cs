namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HiddenSpider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Spider")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.")
        .FlavorText("It wants only to dress you in silk.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.";

            p.Trigger(new OnCastedSpell((c, ctx) =>
                ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Creature && c.Has().Flying));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToCreature(
              power: 3,
              toughness: 5,
              type: t => t.Change(baseTypes: "creature", subTypes: "spider"),
              colors: L(CardColor.Green)),
              () => new AddSimpleAbility(Static.Reach));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}