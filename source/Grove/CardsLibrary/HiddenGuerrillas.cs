namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HiddenGuerrillas : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Guerrillas")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts an artifact spell, if Hidden Guerrillas is an enchantment, Hidden Guerrillas becomes a 5/3 Soldier creature with trample.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts an artifact spell, if Hidden Guerrillas is an enchantment, Hidden Guerrillas becomes a 5/3 Soldier creature with trample.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Enchantment && c.Is().Artifact));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 5,
                toughness: 3,
                type: t => t.Change(baseTypes: "creature", subTypes: "soldier"),
                colors: L(CardColor.Green)),
              () => new AddSimpleAbility(Static.Trample));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}