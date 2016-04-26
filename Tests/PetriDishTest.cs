using GameOfLife.ViewModel;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
  public class PetriDishTest
  {
    private readonly ITimer timer = MockRepository.GenerateStub<ITimer>();

    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 1500000)]
    public void WholeRunTrafficTest()
    {
      var target = new PetriDish(160, 100, timer);

      for (var i = 0; i < 100; i++)
        target.PerformOneStep();
    }

    [Test]
    [DotMemoryUnit(CollectAllocations = true)]
    public void AlgorithmTrafficTest()
    {
      var target = new PetriDish(160, 100, timer);

      var memoryPoint1 = dotMemory.Check();

      for (var i = 0; i < 100; i++)
        target.PerformOneStep();

      dotMemory.Check(memory =>
        Assert.That(
          memory.GetTrafficFrom(memoryPoint1).AllocatedMemory.SizeInBytes,
          Is.LessThan(3000)));
    }
  }
}