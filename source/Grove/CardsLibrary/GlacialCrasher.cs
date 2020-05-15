namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class GlacialCrasher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Glacial Crasher")
        .ManaCost("{4}{U}{U}")
        .Type("Creature — Elemental")
        .Text(
          "{Trample}{I}(If this creature would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}{EOL}Glacial Crasher can't attack unless there is a Mountain on the battlefield.")
        .Power(5)
        .Toughness(5)
        .SimpleAbilities(Static.Trample)
        .StaticAbility(p =>
          {
            p.Modifier(() => new AddSimpleAbility(Static.CannotAttack));
            p.Condition = cp => !cp.PermanentExists(c => c.Is("mountain"));
          });
    }
  }
}