namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class LurkingSkirge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lurking Skirge")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text(
          "When a creature is put into an opponent's graveyard from the battlefield, if Lurking Skirge is an enchantment, Lurking Skirge becomes a 3/2 Imp creature with flying.")
        .FlavorText("They never miss a funeral.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When a creature is put into an opponent's graveyard from the battlefield, if Lurking Skirge is an enchantment, Lurking Skirge becomes a 3/2 Imp creature with flying.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, t, g) =>
                t.OwningCard.Is().Enchantment && c.Is().Creature && c.Owner != t.OwningCard.Controller));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 2,
                type: t => t.Change(baseTypes: "creature", subTypes: "imp"),
                colors: L(CardColor.Black)),
              () => new AddStaticAbility(Static.Flying));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}