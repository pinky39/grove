namespace Grove.Core.Controllers
{
  public interface IProcessDecisionResults<T>
  {
    void ResultProcessed(T results);
  }
}