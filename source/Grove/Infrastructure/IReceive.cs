namespace Grove.Infrastructure
{
  public interface IReceive<in T> : IReceive
  {
    void Receive(T e);
  }

  public interface IReceive {}
}