namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class VeiledSerpent : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veiled Serpent")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veiled Serpent is an enchantment, Veiled Serpent becomes a 4/4 Serpent creature that can't attack unless defending player controls an Island.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.")
        .Cycling("{2}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Serpent is an enchantment, Veiled Serpent becomes a 4/4 Serpent creature that can't attack unless defending player controls an Island.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) => ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 4,
                toughness: 4,
                type: t => t.Change(baseTypes: "creature", subTypes: "serpent"),
                colors: L(CardColor.Blue)),
              () => new AddStaticAbility(Static.CanAttackOnlyIfDefenderHasIslands));      

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}