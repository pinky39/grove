namespace Grove.Infrastructure
{
  using System;
  using System.Linq;
  using System.Reflection;

  public static class TypeExtensions
  {
    public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
    {
      return Attribute.GetCustomAttributes(memberInfo, typeof (TAttribute), true).FirstOrDefault() as TAttribute;
    }

    public static PropertyInfo GetProperty(this MethodInfo methodInfo)
    {
      if (!(methodInfo.IsGetter() || methodInfo.IsSetter()))
        return null;

      return methodInfo.DeclaringType.GetProperty(methodInfo.Name.Substring(4),
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo)
    {
      return Attribute.IsDefined(memberInfo, typeof (TAttribute), true);
    }

    public static bool Implements<TInterface>(this Type type) where TInterface : class
    {
      return type.GetInterfaces().Count(x => x == typeof (TInterface)) > 0;
    }

    public static bool IsAddEventHandler(this MethodInfo methodInfo)
    {
      return methodInfo.Name.StartsWith("add_");
    }

    public static bool IsGetter(this MethodInfo methodInfo)
    {
      return methodInfo.Name.StartsWith("get_");
    }

    public static bool IsSetter(this MethodInfo methodInfo)
    {
      return methodInfo.Name.StartsWith("set_");
    }

    public static bool IsSetterOrGetter(this MethodInfo methodInfo)
    {
      return methodInfo.IsSetter() || methodInfo.IsGetter();
    }
  }
}