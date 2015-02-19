namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;
  using Modifiers;

  public class XathridSlyblade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Xathrid Slyblade")
        .ManaCost("{2}{B}")
        .Type("Creature — Human Assassin")
        .Text("{Hexproof} {I}(This creature can't be the target of spells or abilities your opponents control.){/I}{EOL}{3}{B}: Until end of turn, Xathrid Slyblade loses hexproof and gains first strike and deathtouch. {I}(It deals combat damage before creatures without first strike. Any amount of damage it deals to a creature is enough to destroy it.){/I}")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Hexproof)
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{B}: Until end of turn, Xathrid Slyblade loses hexproof and gains first strike and deathtouch.";

          p.Cost = new PayMana("{3}{B}".Parse());

          p.Effect = () => new ApplyModifiersToSelf(
            () => new RemoveAbility(Static.Hexproof) {UntilEot = true},
            () => new AddStaticAbility(Static.FirstStrike) {UntilEot = true},
            () => new AddStaticAbility(Static.Deathtouch) { UntilEot = true });
        });
    }
  }
}
