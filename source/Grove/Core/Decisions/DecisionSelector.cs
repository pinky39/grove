namespace Grove.Core.Controllers
{
  using System.Reflection;
  using Castle.Facilities.TypedFactory;

  public class DecisionSelector : DefaultTypedFactoryComponentSelector
  {
    protected override string GetComponentName(MethodInfo method, object[] arguments)
    {
      if (method.Name == "CreateHuman")
      {
        return "Human#" + method.GetGenericArguments()[0].Name;
      }
      if (method.Name == "CreateMachine")
      {
        return "Machine#" + method.GetGenericArguments()[0].Name;
      }

      return base.GetComponentName(method, arguments);
    }
  }
}