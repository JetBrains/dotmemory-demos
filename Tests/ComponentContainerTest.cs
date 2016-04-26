using GameOfLife;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;

namespace Tests
{
  public class ComponentContainerTest
  {
    [Test]
    public void ShutdownTest()
    {
      // --arrange
      var target = new ComponentContainer();
      target.CreateMainViewModel();

      // --act
      target.Dispose();

      // --assert
      dotMemory.Check(memory =>
      {
        Assert.That(
          memory
            .GetObjects(where => where.Type.IsNot<ComponentContainer>())
            .GetObjects(where => where.Namespace.Like("GameOfLife.*"))
            .ObjectsCount,
          Is.EqualTo(0));
      });

      dotMemoryApi.SaveCollectedData();
    }    

  }
}