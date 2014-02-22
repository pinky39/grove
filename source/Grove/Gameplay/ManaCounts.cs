namespace Grove.Gameplay
{
  public class ManaCounts
  {
    public ManaCounts(int white, int blue, int black, int red, int green, int multi, int colorless)
    {
      White = white;
      Blue = blue;
      Black = black;
      Red = red;
      Green = green;
      Multi = multi;
      Colorless = colorless;
    }

    public int White { get; private set; }
    public int Blue { get; private set; }
    public int Black { get; private set; }
    public int Red { get; private set; }
    public int Green { get; private set; }
    public int Multi { get; private set; }
    public int Colorless { get; private set; }
  }
}