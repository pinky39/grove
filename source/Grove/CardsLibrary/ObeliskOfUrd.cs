namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class ObeliskOfUrd : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Obelisk of Urd")
        .ManaCost("{6}")
        .Type("Artifact")
        .Text("{Convoke} (Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){EOL}As Obelisk of Urd enters the battlefield, choose a creature type.{EOL}Creatures you control of the chosen type get +2/+2.")
        .Convoke()
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          p.Effect = () => new CreaturesOfChosenTypeGainPT(2, 2, ControlledBy.SpellOwner);
          p.UsesStack = false;
        });
    }
  }
}
