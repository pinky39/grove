namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Costs;
  using Effects;
  using Modifiers;

  public class SoulOfNewPhyrexia : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of New Phyrexia")
        .ManaCost("{6}")
        .Type("Creature — Avatar")
        .Text("{Trample}{EOL}{5}: Permanents you control gain indestructible until end of turn.{EOL}{5}, Exile Soul of New Phyrexia from your graveyard: Permanents you control gain indestructible until end of turn.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Trample)
        .ActivatedAbility(p =>
        {
          p.Text = "{5}: Permanents you control gain indestructible until end of turn.";
          p.Cost = new PayMana("{5}".Parse());

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (e, c) => c.Is().Creature,
            controlledBy: ControlledBy.SpellOwner,
            modifiers: new CardModifierFactory[]
            {
              () => new AddStaticAbility(Static.Indestructible){UntilEot = true}
            }).SetTags(EffectTag.Indestructible);
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{5}, Exile Soul of New Phyrexia from your graveyard: Permanents you control gain indestructible until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{5}".Parse()),
            new Exile(fromGraveyard: true));

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (e, c) => c.Is().Creature,
            controlledBy: ControlledBy.SpellOwner,
            modifiers: new CardModifierFactory[]
            {
              () => new AddStaticAbility(Static.Indestructible){UntilEot = true}
            }).SetTags(EffectTag.Indestructible);

          p.ActivationZone = Zone.Graveyard;
        });
    }
  }
}
