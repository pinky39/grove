namespace Grove.Core.Details.Combat
{
  public interface IAttackerFactory
  {
    Attacker Create(Card card);
  }
}