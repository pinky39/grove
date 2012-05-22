namespace Grove.Infrastructure
{
  public interface INotifyPropertyChangedRaiser
  {
    void RaisePropertyChanged(string propertyName);
  }
}