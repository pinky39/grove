namespace Grove.Core.Details.Cards.Effects
{
  using Mana;

  public class AddManaToPool : Effect
  {
    public IManaAmount Mana { get; set; }

    protected override void ResolveEffect()
    {
      Controller.AddManaToManaPool(Mana);
    }
  }
}