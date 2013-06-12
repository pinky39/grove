namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.DamageHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class UrzasArmor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Urza's Armor")
        .ManaCost("{6}")
        .Type("Artifact")
        .Text("If a source would deal damage to you, prevent 1 of that damage.")
        .FlavorText(
          "Tawnos's blueprints were critical to the creation of my armor. As he once sealed himself in steel, I sealed myself in a walking crypt.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .StaticAbility(p => p.Modifier(() => new AddDamagePrevention(
          modifier => new PreventDamageToTarget(modifier.SourceCard.Controller, maxAmount: 1))));
    }
  }
}