namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Modifiers;

  public class CitanulHierophants : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Citanul Hierophants")
        .ManaCost("{3}{G}")
        .Type("Creature Human Druid")
        .Text("Creatures you control have '{T}: Add {G} to your mana pool.'")
        .FlavorText(
          "From deep in the caves beneath the forest, the hierophants planned the druids' raids against the enemy.")
        .Power(3)
        .Toughness(2)
        .ContinuousEffect(p =>
          {
            p.Selector = (card, ctx) => card.Controller == ctx.You && card.Is().Creature;
            p.Modifier = () =>
              {
                var mp = new ManaAbilityParameters
                  {                    
                    Text = "{T}:  Add {G} to your mana pool.",
                    Priority = ManaSourcePriorities.Creature,                    
                  };

                mp.ManaAmount(Mana.Green);

                return new AddActivatedAbility(new ManaAbility(mp));
              };
          });
    }
  }
}