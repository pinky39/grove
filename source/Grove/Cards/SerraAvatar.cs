namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class SerraAvatar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra Avatar")
        .ManaCost("{4}{W}{W}{W}")
        .Type("Creature Avatar")
        .Text(
          "Serra Avatar's power and toughness are each equal to your life total.{EOL}When Serra Avatar is put into a graveyard from anywhere, shuffle it into its owner's library.")
        .Power(0)
        .Toughness(0)        
        .StaticAbility(p =>
          {
            p.Modifier(() => new ModifyPowerToughnessEqualToControllersLife());
            p.EnabledInAllZones = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Serra Avatar is put into a graveyard from anywhere, shuffle it into its owner's library.";
            p.Trigger(new OnZoneChanged(to: Zone.Graveyard));
            p.Effect = () => new ShuffleIntoLibrary();
          });
    }
  }
}