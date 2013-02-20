namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsToSacrifice : Decisions.SelectCardsToSacrifice
  {
    protected override void ExecuteQuery()
    {
      Result = GetTargets(descending: false);           
    }
  }
}