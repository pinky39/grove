namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class OpalAcrolith : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Acrolith")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.{EOL}{0}: Opal Acrolith becomes an enchantment.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
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

            p.TimingRule(new WhenCardHas(c => c.Is().Creature));
            p.TimingRule(new WhenOwningCardWillBeDestroyed());
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}