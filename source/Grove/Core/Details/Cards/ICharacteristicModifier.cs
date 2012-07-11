namespace Grove.Core.Details.Cards
{
  public interface ICharacteristicModifier<T>
  {
    int Priority { get; }
    T Apply(T value);
  }
}