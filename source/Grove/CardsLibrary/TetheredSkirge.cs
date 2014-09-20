namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class TetheredSkirge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tethered Skirge")
        .ManaCost("{2}{B}")
        .Type("Creature Imp")
        .Text("{Flying}{EOL}Whenever Tethered Skirge becomes the target of a spell or ability, you lose 1 life.")
        .FlavorText("It bites the hand that leads it.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Tethered Skirge becomes the target of a spell or ability, you lose 1 life.";

            p.Trigger(new OnBeingTargetedBySpellOrAbility());
            p.Effect = () => new YouLooseLife(1);                        
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}