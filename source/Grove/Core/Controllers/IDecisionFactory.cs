namespace Grove.Core.Controllers
{
  using System;

  public interface IDecisionFactory
  {
    object Create(Type type);
  }
}