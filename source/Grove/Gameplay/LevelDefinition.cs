namespace Grove.Gameplay
{
  public class LevelDefinition
  {
    public int Min { get; set; }
    public int? Max { get; set; }

    public int Power { get; set; }
    public int Toughness { get; set; }

    public Static StaticAbility { get; set; }
  }
}