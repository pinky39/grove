namespace Grove.Infrastructure
{
  public interface IHashable
  {
    int CalculateHash(HashCalculator calc);
  }
}