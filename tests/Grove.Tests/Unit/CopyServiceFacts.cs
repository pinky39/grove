namespace Grove.Tests.Unit
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;
  using Xunit;

  public class CopyServiceFacts
  {
    [Fact]
    public void CopyInherited()
    {
      var triceratops = new BabyTriceratops{
        Age = 3,
        HasBeenVaxined = true,
        NumOfHorns = 5
      };

      var copyService = new CopyService();
      var triceratopsCopy = copyService.Copy(triceratops);


      Assert.NotSame(triceratops, triceratopsCopy);
      Assert.Equal(triceratops.Age, triceratopsCopy.Age);
      Assert.Equal(triceratops.NumOfHorns, triceratopsCopy.NumOfHorns);
      Assert.Equal(triceratops.HasBeenVaxined, triceratopsCopy.HasBeenVaxined);
    }

    [Fact]
    public void RespectSameReferences()
    {
      var majkl = new Person{Name = "Majkl"};

      var village = new Village(
        new House{
          Age = 15,
          FrontDoor = new Door().Lock(),
          Owner = majkl
        },
        new House{
          Age = 75,
          FrontDoor = new Door(),
          Owner = majkl
        });

      var copyService = new CopyService();
      var villageCopy = copyService.Copy(village);

      Assert.NotSame(villageCopy, village);
      Assert.Equal(2, villageCopy.Count());
      Assert.True(villageCopy.HousesHaveSameOwner);
    }

    [Fact]
    public void DoNotCopyEventsRegistrations()
    {
      var copyService = new CopyService();

      int count = 0;
      var has = new HasEvents();
      has.Raised += delegate { count++; };
      has.Raise();
      
      var hasCopy = copyService.Copy(has);
      hasCopy.Raise();

      Assert.Equal(1, count);
    }

    [Fact]
    public void CopyDictionary()
    {
      var org = new HasDictionary();
      org.Dictionary.Add("dino", new Dinosaur {Age = 5});

      var copy = new CopyService().Copy(org);
      
      Assert.NotSame(org.Dictionary, copy.Dictionary);
      Assert.Equal(5, copy.Dictionary["dino"].Age);
    }
  }

  [Copyable]
  public class HasDictionary
  {
    public Dictionary<string, Dinosaur> Dictionary = new Dictionary<string, Dinosaur>();
  }

  [Copyable]
  public class HasEvents
  {
    public event EventHandler Raised = delegate { };

    public void Raise()
    {
      Raised(this, EventArgs.Empty);
    }
  }

  [Copyable]
  public class Dinosaur
  {
    public int Age { get; set; }
  }

  public class Triceratops : Dinosaur
  {
    public int NumOfHorns { get; set; }
  }

  public class BabyTriceratops : Triceratops
  {
    public bool HasBeenVaxined { get; set; }
  }

  [Copyable]
  public class Village : IEnumerable<House>
  {
    private readonly List<House> _houses = new List<House>();

    public Village(params House[] houses)
    {
      _houses.AddRange(houses);
    }

    private Village() {}

    public bool HousesHaveSameOwner
    {
      get
      {
        Person owner = null;

        foreach (var house in _houses)
        {
          if (owner != null && !owner.Equals(house.Owner))
          {
            return false;
          }
          owner = house.Owner;
        }
        return true;
      }
    }

    public IEnumerator<House> GetEnumerator()
    {
      return _houses.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }


  [Copyable]
  public class House
  {
    public int Age { get; set; }

    public Door FrontDoor { get; set; }
    public Person Owner { get; set; }
  }

  [Copyable]
  public class Person
  {
    public string Name { get; set; }
  }

  [Copyable]
  public class Door
  {
    private readonly DoorLock _lock = new DoorLock();

    public bool IsLocked { get { return _lock.IsLocked; } }

    public bool IsOpen { get; set; }

    public Door Lock()
    {
      _lock.IsLocked = true;
      return this;
    }
  }

  [Copyable]
  public class DoorLock
  {
    public bool IsLocked { get; set; }
  }
}