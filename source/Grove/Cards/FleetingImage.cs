namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class FleetingImage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fleeting Image")
        .ManaCost("{2}{U}")
        .Type("Creature Illusion")
        .Text("{Flying}{EOL}{1}{U}: Return Fleeting Image to its owner's hand.")
        .FlavorText(
          "Horas swore off drinking, but after meeting the creature a second time he decided he'd better start again.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{U}: Return Fleeting Image to its owner's hand.";
            p.Cost = new PayMana("{1}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new Gameplay.Effects.ReturnToHand(returnOwningCard: true);

            p.TimingRule(new WhenOwningCardWillBeDestroyed());
            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          }
        );
    }
  }
}