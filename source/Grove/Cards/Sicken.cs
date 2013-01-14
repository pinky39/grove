namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Targeting;

  public class Sicken : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sicken")
        .ManaCost("{B}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets -1/-1.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Urza dared to attack Phyrexia. Slowly, it retaliated.")  
        .OverrideScore(100)
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = Effect<Core.Effects.Attach>(e =>
              {
                e.ToughnessReduction = 1;
                e.Modifiers(Modifier<AddPowerAndToughness>(m =>
                  {
                    m.Power = -1;
                    m.Toughness = -1;
                  }));
              });
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.ReduceToughness(1);
          });
    }
  }
}