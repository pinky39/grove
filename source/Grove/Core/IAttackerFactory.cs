namespace Grove.Core
{
  public interface IAttackerFactory {
    Attacker Create(Card card);
  }
}