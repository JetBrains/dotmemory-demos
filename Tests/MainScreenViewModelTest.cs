using System;
using System.Collections.Generic;
using GameOfLife.ViewModel;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;

namespace Tests
{
  public class Hui
  {
    [Test]
    public void RemovePetriDishTest()
    {
      // --arrange
      var target = new MainScreenViewModel(0);
      target.AddPetriDishCommand.Execute(null);

      // --act
      target.RemovePetriDishCommand.Execute(null);

      // --assert
      Assert.That(target.PetriDishesCollection, Has.Count.EqualTo(0));

      // --memory
      dotMemory.Check(memory =>
        Assert.That(
          memory.GetObjects(where => where.Type.Is<PetriDish>()).ObjectsCount,
          Is.EqualTo(0)));
    }

    [Test]
    [DotMemoryUnit(CollectAllocations = true)]
    public void EventHandlerLeakTest()
    {
      // --arrange
      var target = new MainScreenViewModel();

      // --act
      target = null;

      // --assert
      dotMemory.Check(memory =>
        Assert.That(
          memory.GetObjects(where => where.Namespace.Like("GameOfLife.*")).ObjectsCount,
          Is.EqualTo(0)));

      dotMemoryApi.SaveCollectedData();
    }

    [Test]
    [DotMemoryUnit(CollectAllocations = true)]
    public void Test()
    {
      var target = new List<Hz>();
      target = null;

      dotMemory.Check(memory =>
        Assert.That(
          memory.GetObjects(where => where.Type.Is<Hz[]>()).ObjectsCount,
          Is.EqualTo(0)));
      Console.WriteLine(typeof(Hz[]).Namespace);
    }

    class Hz
    {
      
    }
  }


}