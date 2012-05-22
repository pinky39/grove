namespace Grove.Infrastructure
{
  using System;
  using System.Linq.Expressions;
  using System.Reflection;

  public delegate object ObjectActivator(params object[] args);

  public static class Activation
  {
    public static ConstructorInfo GetParameterlessConstructor(this Type type)
    {
      return type.GetConstructor(
        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
        CallingConventions.Any,
        Type.EmptyTypes, null);
    }

    // Faster way to dynamicly create objects
    // http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/
    public static ObjectActivator GetActivator(ConstructorInfo ctor)
    {
      var paramsInfo = ctor.GetParameters();

      var param = Expression.Parameter(typeof(object[]), "args");

      var argsExp = new Expression[paramsInfo.Length];

      for (var i = 0; i < paramsInfo.Length; i++)
      {
        Expression index = Expression.Constant(i);
        var paramType = paramsInfo[i].ParameterType;

        Expression paramAccessorExp =
          Expression.ArrayIndex(param, index);

        Expression paramCastExp =
          Expression.Convert(paramAccessorExp, paramType);

        argsExp[i] = paramCastExp;
      }

      var newExp = Expression.New(ctor, argsExp);

      var lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

      var compiled = (ObjectActivator)lambda.Compile();
      return compiled;
    }    
  }
}