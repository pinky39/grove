namespace Grove.Ui.MessageBox
{
  using System;
  using System.Windows;
  using Infrastructure;

  public class ViewModel : IClosable
  {
    public ViewModel(string message, string title, MessageBoxButton buttons, DialogType dialogType = DialogType.Large)
    {
      Message = message;
      Title = title;
      DialogType = dialogType;
      Buttons = buttons;
      Icon = "question.png";
    }


    public MessageBoxButton Buttons { get; private set; }
    public DialogType DialogType { get; private set; }

    public string Icon { get; private set; }

    public bool IsOk { get { return Buttons == MessageBoxButton.OK; } }

    public bool IsYesNo { get { return Buttons == MessageBoxButton.YesNo; } }

    public string Message { get; private set; }
    public MessageBoxResult Result { get; private set; }
    public string Title { get; private set; }
    public event EventHandler Closed = delegate { };

    public void Close()
    {
      Closed(this, EventArgs.Empty);
    }

    public void No()
    {
      Close(MessageBoxResult.No);
    }

    public void Ok()
    {
      Close(MessageBoxResult.OK);
    }

    public void Yes()
    {
      Close(MessageBoxResult.Yes);
    }

    private void Close(MessageBoxResult result)
    {
      Result = result;
      Close();
    }
  }
}