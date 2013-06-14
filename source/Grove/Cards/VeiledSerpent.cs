namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class VeiledSerpent : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Veiled Serpent")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veiled Serpent is an enchantment, Veiled Serpent becomes a 4/4 Serpent creature that can't attack unless defending player controls an Island.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.")
        .Cycling("{2}")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Serpent is an enchantment, Veiled Serpent becomes a 4/4 Serpent creature that can't attack unless defending player controls an Island.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) => ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 4,
                toughness: 4,
                type: "Creature Serpent",
                colors: L(CardColor.Blue)),
              () => new AddStaticAbility(Static.CanAttackOnlyIfDefenderHasIslands));      

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}