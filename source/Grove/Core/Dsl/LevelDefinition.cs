namespace Grove.Core.Dsl
{
  using Details.Cards;

  public class LevelDefinition
  {
    public int Min { get; set; }
    public int? Max { get; set; }

    public int Power { get; set; }
    public int Thoughness { get; set; }

    public Static StaticAbility { get; set; }
  }
}