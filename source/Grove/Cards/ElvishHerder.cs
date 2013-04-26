namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

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
            p.Cost = new PayMana(Mana.Green, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Trample) {UntilEot = true});
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new DeclareBlockers());
            p.TargetingRule(new GainEvasion(x => !x.Has().Trample && x.Power >= 4));
          });
    }
  }
}