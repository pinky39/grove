namespace Grove.Core.Controllers.Machine
{
  using System;

  public class SelectCardsToSacrifice : Controllers.SelectCardsToSacrifice
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: false);
    }
  }
}