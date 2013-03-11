namespace Grove.Core.Effects
{
  using Ai;
  using Decisions;
  using Zones;

  public class OpponentSacrificesCreatures : Effect
  {
    private readonly int _count;

    private OpponentSacrificesCreatures() {}

    public OpponentSacrificesCreatures(int count)
    {
      _count = count;
      Category = EffectCategories.Destruction;
    }

    protected override void ResolveEffect()
    {
      Enqueue<SelectCardsToSacrifice>(
        controller: Controller.Opponent,
        init: p =>
          {
            p.MinCount = _count;
            p.MaxCount = _count;
            p.Validator = card => card.Is().Creature;
            p.Zone = Zone.Battlefield;
            p.Text = FormatText("Select creature(s) to sacrifice");
            p.OwningCard = Source.OwningCard;
          });
    }
  }
}