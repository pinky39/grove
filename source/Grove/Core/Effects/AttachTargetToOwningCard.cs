namespace Grove.Effects
{
  using Modifiers;

  public class AttachTargetToOwningCard : Effect
  {
    private readonly bool _gainControl;

    private AttachTargetToOwningCard() {}

    public AttachTargetToOwningCard(bool gainControl = true)
    {
      _gainControl = gainControl;
    }

    protected override void ResolveEffect()
    {
      var enchantment = Target.Card();
      enchantment.EnchantWithoutPayingCost(Source.OwningCard);

      if (_gainControl && enchantment.Controller != Controller)
      {
        GainControl(enchantment);
      }
    }

    private void GainControl(Card enchantment)
    {
      var sourceModifier = new ChangeController(Controller);

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      enchantment.AddModifier(sourceModifier, p);
    }
  }
}