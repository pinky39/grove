namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Pacifism : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Pacifism")
        .ManaCost("{1}{W}")
        .Type("Enchantment - Aura")
        .Text("Enchant creature{EOL}Enchanted creature can't attack or block.")
        .FlavorText("'Fight? I cannot. I do not care if I live or die, so long as I can rest.'{EOL}—Urza, to Serra")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.CannotBlock),
              () => new AddStaticAbility(Static.CannotAttack))
              {
                Category = EffectCategories.Destruction
              };

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new FirstMain());
            p.TargetingRule(new Destroy());
          });
    }
  }
}