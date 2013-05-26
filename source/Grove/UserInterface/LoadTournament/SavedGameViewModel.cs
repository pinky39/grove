namespace Grove.UserInterface.LoadTournament
{
  using System;

  public class SavedGameViewModel
  {
    public string Filename { get; set; }
    public string Description { get; set; }
    public DateTime LastSave { get; set; }
    public bool IsOdd { get; set; }
  }
}