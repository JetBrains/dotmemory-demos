using GameOfLife.ViewModel;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
  public class PetriDishTest
  {
    const int Mb = 1024*1024;

    private readonly ITimer timer = MockRepository.GenerateStub<ITimer>();

    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 2*Mb)] // --assert
    public void WholeRunTraffic()
    {
      // --act
      var target = new PetriDish(160, 100, timer);

      for (var i = 0; i < 100; i++)
        target.PerformOneStep();
    }

    [Test]
    [DotMemoryUnit(CollectAllocations = true)]
    public void AlgorithmTraffic()
    {
      // --arrange
      var target = new PetriDish(160, 100, timer);

      var memoryPoint1 = dotMemory.Check();

      // --act
      for (var i = 0; i < 100; i++)
        target.PerformOneStep();

      // --assert
      dotMemory.Check(memory =>
        Assert.That(memory.GetTrafficFrom(memoryPoint1)
          .AllocatedMemory
            .ObjectsCount,
          Is.LessThan(100)));
    }

    [Test]
    public void DontRecreateArrays()
    {
      // --arrange
      var target = new PetriDish(160, 100, timer);

      var memoryPoint1 = dotMemory.Check();

      // --act
      target.PerformOneStep();

      // --assert
      dotMemory.Check(memory =>
        Assert.That(memory.GetDifference(memoryPoint1)
          .GetNewObjects(where => where.Type.Is<Cell[,]>())
            .ObjectsCount,
          Is.EqualTo(0)));
    }
  }
}