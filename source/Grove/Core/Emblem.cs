namespace Grove
{
  using Infrastructure;

  [Copyable]
  public class Emblem
  {
    public readonly string Text;
    public readonly int Score;

    private Emblem() {}

    public Emblem(string text, int score)
    {
      Text = text;
      Score = score;
    }
  }
}