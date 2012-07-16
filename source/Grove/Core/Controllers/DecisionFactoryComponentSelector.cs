namespace Grove.Core.Controllers
{
  using System;
  using System.Reflection;
  using Castle.Facilities.TypedFactory;

  public class DecisionFactoryComponentSelector : DefaultTypedFactoryComponentSelector
  {
    protected override Type GetComponentType(MethodInfo method, object[] arguments)
    {
      return (Type)arguments[0];
    }        
  }
}