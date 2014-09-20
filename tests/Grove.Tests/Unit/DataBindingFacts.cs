namespace Grove.Tests.Unit
{
  using System.ComponentModel;
  using Grove.Infrastructure;
  using Xunit;

  public class DataBindingFacts
  {
    [Fact]
    public void UpdateViaMethod()
    {
      var changed = false;
      var dog = Bindable.Create<Dog>();
      var notify = dog as INotifyPropertyChanged;

      notify.PropertyChanged += (s, e) =>
        {
          if (e.PropertyName == "Description")
            changed = true;
        };

      dog.ChangeDestription();

      Assert.True(changed);
    }

    [Fact]
    public void UpdateViaProperty()
    {
      var changed = false;
      var dog = Bindable.Create<Dog>();
      var notify = dog as INotifyPropertyChanged;

      notify.PropertyChanged += (s, e) =>
        {
          if (e.PropertyName == "Description")
            changed = true;
        };

      dog.Age = 5;

      Assert.True(changed);
    }

    public class Dog
    {
      [Updates("Description")]
      public virtual int Age { get; set; }

      public virtual string Description { get { return "I am " + Age + " years old."; } }

      [Updates("Description")]
      public virtual void ChangeDestription() {}
    }
  }
}