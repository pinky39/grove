namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class ElvishHerder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elvish Herder")
        .ManaCost("{G}")
        .Type("Creature Elf")
        .Text("{G}: Target creature gains trample until end of turn.")
        .FlavorText(
          "Before Urza and Mishra came to Argoth, the herders prevented their creatures from stampeding. During the war, they encouraged it.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{G}: Target creature gains trample until end of turn.";
            p.Cost = new PayMana(Mana.Green, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Trample) {UntilEot = true});
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TimingRule(new OnYourTurn(Step.DeclareBlockers));
            p.TargetingRule(new EffectBigWithoutEvasions(x => !x.Has().Trample && x.Power >= 4));
          });
    }
  }
}