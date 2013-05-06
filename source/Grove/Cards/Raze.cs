namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class Raze : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Raze")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("As an additional cost to cast Raze, sacrifice a land.{EOL}Destroy target land.")
        .FlavorText("The viashino believe that the oldest mountains hate everyone equally.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana(Mana.Red, ManaUsage.Spells),
              new Sacrifice());

            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddCost(trg =>
              {
                trg.Is.Card(c => c.Is().Land, ControlledBy.SpellOwner).On.Battlefield();
                trg.Message = "Select a land to sacrifice.";
              });

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card(c => c.Is().Land).On.Battlefield();
                trg.Message = "Select a land to destroy.";
              });

            p.TimingRule(new FirstMain());
            p.TargetingRule(new SacrificeTargetToDestroyTarget());
          });
    }
  }
}