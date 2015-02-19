namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SpiritBonds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Spirit Bonds")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text("Whenever a nontoken creature enters the battlefield under your control, you may pay {W}. If you do, put a 1/1 white Spirit creature token with flying onto the battlefield.{EOL}{1}{W}, Sacrifice a Spirit: Target non-Spirit creature gains indestructible until end of turn. {I}(Damage and effects that say \"destroy\" don't destroy it.){/I}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever a nontoken creature enters the battlefield under your control, you may pay {W}. If you do, put a 1/1 white Spirit creature token with flying onto the battlefield.";
          p.Trigger(new OnZoneChanged(
            to: Zone.Battlefield,
            filter: (c, a, g) => a.OwningCard.Controller == c.Controller && c.Is().Creature && !c.Is().Token));

          p.Effect = () => new PayManaPlayEffect(Mana.White, new CreateTokens(
            count: 1,
            token: Card
              .Named("Spirit")
              .Power(1)
              .Toughness(1)
              .Type("Token Creature - Spirit")
              .Text("{Flying}")
              .Colors(CardColor.White)
              .SimpleAbilities(Static.Flying)));

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{W}, Sacrifice a Spirit: Target non-Spirit creature gains indestructible until end of turn.";

          p.Cost = new AggregateCost(
              new PayMana("{1}{W}".Parse()),
              new Sacrifice());

          p.TargetSelector.AddCost(trg =>
          {
            trg.Is.Card(c => c.Is("Spirit"), ControlledBy.SpellOwner).On.Battlefield();
            trg.Message = "Select a Spirit to sacrifice.";
          });

          p.TargetSelector.AddEffect(trg =>
          {
            trg.Is.Card(c => c.Is().Creature && !c.Is().Token).On.Battlefield();
            trg.Message = "Select a creature to gain indestructible.";
          });

          p.Effect = () => new ApplyModifiersToTargets(
                () => new AddStaticAbility(Static.Indestructible) { UntilEot = true }).SetTags(
                  EffectTag.Indestructible);

          p.TargetingRule(new CostSacrificeEffectGiveIndestructible());
        });
    }
  }
}
