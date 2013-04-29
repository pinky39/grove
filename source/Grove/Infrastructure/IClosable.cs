namespace Grove.Infrastructure
{
  using System;

  public interface IClosable
  {
    event EventHandler Closed;
    void Close();
  }
}