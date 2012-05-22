namespace Grove.Core.Controllers.Human
{
  using System.Windows;
  using Ui.Shell;

  public class TakeMulligan : Controllers.TakeMulligan
  {
    public IShell Shell { get; set; }
    
    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        title: "Mulligan",
        message: "Do you want to improve your hand by taking a mulligan?",
        buttons: MessageBoxButton.YesNo);
      
      Result = result == MessageBoxResult.Yes;            
    }
  }
}