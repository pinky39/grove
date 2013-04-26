namespace Grove.Ui.SelectDeck
{
  using System;
  using Gameplay.Deck;

  public class Configuration
  {
    public string ScreenTitle { get; set; }
    public string ForwardText { get; set; }
    public Action<Deck> Forward { get; set; }
    public IIsDialogHost PreviousScreen { get; set; }
  }
}