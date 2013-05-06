namespace Grove.UserInterface.SaveDeckAs
{
  using Infrastructure;

  public class ViewModel
  {
    [Updates("CanSave")]
    public virtual string DeckName { get; set; }

    public bool WasCanceled { get; set; }

    public bool CanSave { get { return !string.IsNullOrEmpty(DeckName); } }

    public void Save()
    {
      this.Close();
    }

    public void Cancel()
    {
      WasCanceled = true;
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}