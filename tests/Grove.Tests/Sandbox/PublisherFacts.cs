namespace Grove.Tests.Sandbox
{
  using System.Reflection;
  using Grove.Infrastructure;
  using PublisherTest;
  using Xunit;


  public class PublisherFacts
  {
    [Fact]
    public void HandleEvent()
    {
      var publisher = new Publisher(Assembly.GetExecutingAssembly(), typeof (Person).Namespace);
      publisher.Initialize( new ChangeTracker());

      var aunt1 = new Aunt();
      var aunt2 = new Aunt();
      var uncle = new Uncle();

      publisher.Subscribe(aunt1);
      publisher.Subscribe(aunt2);
      publisher.Subscribe(uncle);

      publisher.Publish(new Newspaper());
      publisher.Publish(new Newspaper());
      publisher.Publish(new Ticket());

      Assert.Equal(2, aunt1.NewspaperCount);
      Assert.Equal(1, aunt1.TicketCount);
      Assert.Equal(2, aunt2.NewspaperCount);
      Assert.Equal(1, aunt2.TicketCount);
      Assert.Equal(0, uncle.TicketCount);
      Assert.Equal(2, uncle.NewspaperCount);
    }
  }

  namespace PublisherTest
  {
    public class Newspaper
    {     
    }

    public class Ticket
    {      
    }
    
    public class Person
    {
      public int NewspaperCount;
      public int TicketCount;
    }
    
    public class Aunt : Person, IReceive<Newspaper>, IReceive<Ticket>
    {
      public void Receive(Newspaper message)
      {
        NewspaperCount++;
      }

      public void Receive(Ticket message)
      {
        TicketCount++;
      }
    }  

    public class Uncle : Person, IReceive<Newspaper>
    {
      public void Receive(Newspaper message)
      {
        NewspaperCount++;
      }
    }
  }
}