namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class ElvishHerder : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Cost = new PayMana(ManaAmount.Green, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Trample) {UntilEot = true});
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new DeclareBlockers());            
            p.TargetingRule(new GainEvasion(x => !x.Has().Trample && x.Power >= 4));
          });
    }
  }
}