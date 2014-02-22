namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

  public class YawgmothsEdict : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yawgmoth's Edict")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever an opponent casts a white spell, that player loses 1 life and you gain 1 life.")
        .FlavorText("Phyrexia's purity permits no other.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent casts a white spell, that player loses 1 life and you gain 1 life.";
            p.Trigger( new OnCastedSpell( (ability, spell) => 
              spell.Controller != ability.SourceCard.Controller && spell.HasColor(CardColor.White)));
            
            p.Effect = () => new ControllerGainsLifeOpponentLoosesLife(1, 1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });

    }
  }
}