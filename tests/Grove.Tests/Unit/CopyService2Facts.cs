namespace Grove.Tests.Unit
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Grove.Infrastructure;
  using UserInterface;
  using Xunit;
  
  public class CopyService2Facts
  {
    [Fact]
    public void Copy()
    {
      var group = new Group();
      
      var person1 = new Person(group.Messenger) {Age = 30, Name = "Silk"};
      var person2 = new Person(group.Messenger) { Age = 35, Name = "Mar" };

      person1.Relative = person2;
      person2.Relative = person1;

      group.Persons.Add(person1);
      group.Persons.Add(person2);
      

      var groupCopy = (Group) CopyService2.Copy(group);                  

      groupCopy.Messenger.MakeAnouncement();      

      Assert.Equal(0, group.Persons[0].GreetedCount);
      Assert.Equal(1, groupCopy.Persons[0].GreetedCount);            
    }

    [Fact]
    public void CheckThatAllCopyableTypesHaveSerializableAttribute()
    {
      var copyableTypes = Assembly.GetAssembly(typeof (CopyService)).GetTypes()
        .Where(x => x.HasAttribute<CopyableAttribute>())        
        .Where(x => !x.Namespace.StartsWith(typeof(ViewModelBase).Namespace))
        .ToList();

      var violators = new List<string>();

      foreach (var copyableType in copyableTypes)
      {
        var serializableAttribute = copyableType.GetAttribute<SerializableAttribute>(inherit: false);

        if (serializableAttribute == null)
        {
          violators.Add(copyableType.ToString());
        }
      }

      if (violators.Count > 0)
      {
        Console.WriteLine("Following types are marked with [Copyable] but don't have [Serializable] attribute:\n");

        foreach (var violator in violators)
        {
          Console.WriteLine(violator);
        }

        Assert.False(true);
      }
    }

    [Serializable]
    public class Group
    {
      public Anouncer Messenger = new Anouncer();
      public List<Person> Persons = new List<Person>();
    }

    [Serializable]
    public class Anouncer
    {
      public EventHandler Anounced = delegate { };      
      
      public void MakeAnouncement()
      {
        Anounced(this, EventArgs.Empty);
      }
    }
    
    [Serializable]
    public class Person
    {
      public int Age;
      public EventHandler Greeted = delegate { };
      public string Name;
      public Person Relative { get; set; }

      public Person(Anouncer anouncer)
      {
        anouncer.Anounced += delegate { GreetedCount++; };
      }

      public int GreetedCount { get; set; }     
    }
  }
}