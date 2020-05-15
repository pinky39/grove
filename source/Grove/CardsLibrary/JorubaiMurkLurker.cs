namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class JorubaiMurkLurker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Jorubai Murk Lurker")
          .ManaCost("{2}{U}")
          .Type("Creature — Leech")
          .Text("Jorubai Murk Lurker gets +1/+1 as long as you control a Swamp.{EOL}{1}{B}: Target creature gains lifelink until end of turn. {I}(Damage dealt by the creature also causes its controller to gain that much life.){/I}")
          .Power(1)
          .Toughness(3)
          .StaticAbility(p =>
          {
            p.Modifier(() => new AddPowerAndToughness(1, 1));
            p.Condition = cond => cond.OwnerControlsPermanent(c => c.Is("swamp"));
          })
          .ActivatedAbility(p =>
          {
            p.Text = "{1}{B}: Target creature gains lifelink until end of turn.";

            p.Cost = new PayMana("{1}{B}".Parse());

            p.Effect = () => new ApplyModifiersToTargets(() => new AddSimpleAbility(Static.Lifelink) { UntilEot = true });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectYourAttackerOrBlocker());
            p.TimingRule(new Any(new AfterYouDeclareAttackers(), new AfterYouDeclareBlockers()));
          });
    }
  }
}
