namespace Grove.Core
{
  using Infrastructure;

  public interface IHashable
  {
    int CalculateHash(HashCalculator calc);
  }
}