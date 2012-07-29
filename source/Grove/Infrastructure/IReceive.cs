namespace Grove.Infrastructure
{    
  public interface IReceive {}
  
  public interface IReceive<in T> : IReceive
  {
    void Receive(T message);
  }

  public interface IOrderedReceive<in T> : IReceive<T>
  {
     int Order { get; }
  }
}