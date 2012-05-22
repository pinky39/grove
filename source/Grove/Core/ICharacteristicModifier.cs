namespace Grove.Core
{
  public interface ICharacteristicModifier<T>
  {
    int Priority { get; }
    T Apply(T value);
  }   
}