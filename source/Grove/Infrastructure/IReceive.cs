namespace Grove.Infrastructure
{
  public interface IReceive<in T> : IReceive
  {
    void Receive(T message);
  }

  public interface IReceive {}
}