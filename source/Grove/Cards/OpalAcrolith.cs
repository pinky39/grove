namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class OpalAcrolith : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Acrolith")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.{EOL}{0}: Opal Acrolith becomes an enchantment.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(() => new Gameplay.Modifiers.ChangeToCreature(
              power: 2,
              toughness: 4,
              type: "Creature Soldier",
              colors: L(CardColor.White)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{0}: Opal Acrolith becomes an enchantment.";
            p.Cost = new PayMana(Mana.Zero, ManaUsage.Abilities);
            p.Effect = () => new RemoveModifier(typeof (Gameplay.Modifiers.ChangeToCreature));

            p.TimingRule(new OwningCardHas(c => c.Is().Creature));
            p.TimingRule(new OwningCardWillBeDestroyed());
          });
    }
  }
}