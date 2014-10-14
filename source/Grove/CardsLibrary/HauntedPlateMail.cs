namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Costs;
  using Effects;
  using Infrastructure;
  using Modifiers;

  public class HauntedPlateMail : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Haunted Plate Mail")
        .ManaCost("{4}")
        .Type("Artifact — Equipment")
        .Text("Equipped creature gets +4/+4.{EOL}{0}: Until end of turn, Haunted Plate Mail becomes a 4/4 Spirit artifact creature that's no longer an Equipment. Activate this ability only if you control no creatures.{EOL}Equip {4}({4}: Attach to target creature you control. Equip only as a sorcery.)")
        .ActivatedAbility(p =>
        {
          p.Text = "Equip {4} ({4}: Attach to target creature you control. Equip only as a sorcery.)";

          p.Cost = new PayMana(4.Colorless(), ManaUsage.Abilities);

          p.Effect = () => new Attach(
            () => new AddPowerAndToughness(4, 4)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.IsEquip = true;
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{0}: Until end of turn, Haunted Plate Mail becomes a 4/4 Spirit artifact creature that's no longer an Equipment.";

          p.Cost = new PayMana(Mana.Zero, ManaUsage.Abilities);

          p.Effect = () => new ApplyModifiersToSelf(
            () => new ChangeToCreature(
              power: 4, 
              toughness: 4, 
              type: "Artifact Creature - Spirit",
              colors: L(CardColor.Colorless)));

          p.Condition = ability => ability.OwningCard.Controller.Battlefield.Creatures.None();
        });
    }
  }
}
