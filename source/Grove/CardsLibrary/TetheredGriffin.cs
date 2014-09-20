namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Infrastructure;
  using Triggers;

  public class TetheredGriffin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tethered Griffin")
        .ManaCost("{W}")
        .Type("Creature Griffin")
        .Text("{Flying}{EOL}When you control no enchantments, sacrifice Tethered Griffin.")
        .FlavorText("Poorly trained griffins sometimes attack the noble audiences that gather to watch their progress.")
        .Power(2)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When you control no enchantments, sacrifice Tethered Griffin.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) => ability.OwningCard.Controller
                .Battlefield.None(x => x.Is().Enchantment)));

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}