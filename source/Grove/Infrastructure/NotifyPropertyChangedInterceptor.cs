namespace Grove.Infrastructure
{
  using System.Collections;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Reflection;
  using Caliburn.Micro;
  using Castle.DynamicProxy;

  public class NotifyPropertyChangedInterceptor : IInterceptor
  {
    private static readonly string[] PropertiesThatGetChangedWhenCollectionIsModified = {"Count", "IsEmpty"};

    private NotifyCollectionChangedEventHandler _collectionChangedSubscribers = delegate { };
    private PropertyChangedEventHandler _propertyChangedSubscribers = delegate { };

    public void Intercept(IInvocation invocation)
    {
      var result =
        InterceptNotifyPropertyChanged(invocation) ??
          InterceptNotifyPropertyChangedRaiser(invocation) ??
            InterceptNotifyCollectionChanged(invocation) ??
              InterceptCallsToProxiedType(invocation);
    }

    private static int GetItemIndex(IEnumerable items, object item)
    {
      var index = 0;
      foreach (var element in items)
      {
        if (element == item)
        {
          return index;
        }
        index++;
      }
      return -1;
    }    

    private object InterceptCallsToProxiedType(IInvocation invocation)
    {
      object removedItem = null;
      var removedIndex = 0;

      if (invocation.InvocationTarget is IEnumerable)
      {
        var collection = (IEnumerable) invocation.InvocationTarget;
        
        switch (invocation.Method.Name)
        {
          case ("Remove"):
            {
              removedItem = invocation.Arguments[0];
              removedIndex = GetItemIndex(collection, removedItem);
              break;
            }
          case ("RemoveFirst"):
            {
              removedItem = collection.FirstElement();
              removedIndex = 0;
              break;
            }

          case ("RemoveLast"):
            {
              removedItem = collection.FirstElement();
              removedIndex = collection.CountElements();
              break;
            }            
        }                        
      }

      invocation.Proceed();

      if (invocation.Method.IsGetter())
        return Chaining.Stop;

      if (invocation.Method.IsSetter())
      {
        var propertyName = invocation.Method.Name.Substring(4);

        Execute.OnUIThread(() =>
          _propertyChangedSubscribers
            (invocation.InvocationTarget, new PropertyChangedEventArgs(propertyName)));
      }
      else if (invocation.InvocationTarget is IEnumerable)
      {
        if (invocation.Method.Name.StartsWith("Add"))
        {
          var addedItem = invocation.Arguments[0];
          var addedIndex = GetItemIndex((IEnumerable) invocation.InvocationTarget, addedItem);

          Execute.OnUIThread(() =>
            _collectionChangedSubscribers(invocation.InvocationTarget,
              new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItem, addedIndex)));

          RaisePropertyChangedEventForPropertiesThatGetModifiedWhenCollectionChanges(invocation);
        }
        else if (invocation.Method.Name.StartsWith("Remove"))
        {
          if (removedIndex != -1)
          {
            Execute.OnUIThread(() =>
              _collectionChangedSubscribers(invocation.InvocationTarget,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, removedIndex))
              );

            RaisePropertyChangedEventForPropertiesThatGetModifiedWhenCollectionChanges(invocation);
          }
        }
        else if (invocation.Method.Name.StartsWith("Clear") || invocation.Method.Name.StartsWith("Shuffle"))
        {
          Execute.OnUIThread(() =>
            _collectionChangedSubscribers(invocation.InvocationTarget,
              new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
            );

          RaisePropertyChangedEventForPropertiesThatGetModifiedWhenCollectionChanges(invocation);
        }
      }

      var memberToInspect = invocation.Method.IsSetter()
        ? (MemberInfo) invocation.Method.GetProperty()
        : invocation.Method;

      if (memberToInspect.HasAttribute<UpdatesAttribute>())
      {
        var updatesAttribute = memberToInspect.GetAttribute<UpdatesAttribute>();
        foreach (var propertyName in updatesAttribute.PropertyNames)
        {
          var propName = propertyName;

          Execute.OnUIThread(() => _propertyChangedSubscribers(
            invocation.Proxy, new PropertyChangedEventArgs(propName)));
        }
      }

      return Chaining.Stop;
    }

    private object InterceptNotifyCollectionChanged(IInvocation invocation)
    {
      if (invocation.Method.DeclaringType != typeof (INotifyCollectionChanged))
        return Chaining.Continue;

      var notifyCollectionChangedEventHandler = (NotifyCollectionChangedEventHandler) invocation.Arguments[0];
      if (invocation.Method.IsAddEventHandler())
      {
        _collectionChangedSubscribers += notifyCollectionChangedEventHandler;
      }
      else
      {
        _collectionChangedSubscribers -= notifyCollectionChangedEventHandler;
      }
      return Chaining.Stop;
    }


    private object InterceptNotifyPropertyChanged(IInvocation invocation)
    {
      if (invocation.Method.DeclaringType != typeof (INotifyPropertyChanged))
        return Chaining.Continue;

      var propertyChangedEventHandler = (PropertyChangedEventHandler) invocation.Arguments[0];
      if (invocation.Method.IsAddEventHandler())
      {
        _propertyChangedSubscribers += propertyChangedEventHandler;
      }
      else
      {
        _propertyChangedSubscribers -= propertyChangedEventHandler;
      }
      return Chaining.Stop;
    }

    private object InterceptNotifyPropertyChangedRaiser(IInvocation invocation)
    {
      if (invocation.Method.DeclaringType != typeof (INotifyPropertyChangedRaiser))
        return Chaining.Continue;

      var propertyName = (string) invocation.Arguments[0];
      Execute.OnUIThread(() =>
        _propertyChangedSubscribers(invocation.Proxy, new PropertyChangedEventArgs(propertyName)));

      return Chaining.Stop;
    }

    private void RaisePropertyChangedEventForPropertiesThatGetModifiedWhenCollectionChanges(IInvocation invocation)
    {
      foreach (var propertyName in PropertiesThatGetChangedWhenCollectionIsModified)
      {
        var name = propertyName;
        Execute.OnUIThread(() =>
          _propertyChangedSubscribers(
            invocation.InvocationTarget, new PropertyChangedEventArgs(name)));
      }
    }
  }
}