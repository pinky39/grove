namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class YawgmothsEdict : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Yawgmoth's Edict")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever an opponent casts a white spell, that player loses 1 life and you gain 1 life.")
        .FlavorText("Phyrexia's purity permits no other.")
        .Cast(p => p.TimingRule(new FirstMain()))
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