namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class GargoyleSentinel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gargoyle Sentinel")
        .ManaCost("{3}")
        .Type("Artifact Creature — Gargoyle")
        .Text(
          "{Defender}{I}(This creature can't attack.){/I}{EOL}{3}: Until end of turn, Gargoyle Sentinel loses defender and gains flying.")
        .FlavorText(
          "The beating of a gargoyle's wings on the air is like the cracking of stones. Intruders who rouse a gargoyle are certain to hear the cracking of bones.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Defender)
        .ActivatedAbility(p =>
          {
            p.Text = "{3}: Until end of turn, Gargoyle Sentinel loses defender and gains flying.";

            p.Cost = new PayMana(3.Colorless(), ManaUsage.Abilities);

            p.Effect = () => new ApplyModifiersToSelf(
              () => new RemoveAbility(Static.Defender) {UntilEot = true},
              () => new AddStaticAbility(Static.Flying) {UntilEot = true});

            p.TimingRule(new BeforeYouDeclareAttackers());
          });
    }
  }
}