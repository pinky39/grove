namespace Grove.Tests.Unit
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Runtime.Serialization;
  using System.Runtime.Serialization.Formatters.Binary;
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

  public class CopyService2
  {
    public static object Copy(object obj)
    {
      var stream = new MemoryStream();

      var formatter = new BinaryFormatter
        {
          Context = new StreamingContext(StreamingContextStates.Clone)
        };

      formatter.Serialize(stream, obj);
      stream.Position = 0;


      return formatter.Deserialize(stream);
    }
  }
}