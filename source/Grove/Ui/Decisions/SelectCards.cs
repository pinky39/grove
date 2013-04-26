namespace Grove.Ui.Decisions
{
  public class SelectCards : Gameplay.Decisions.SelectCards
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}