using GameOfLife;
using GameOfLife.ViewModel;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace Tests
{
  public class MainScreenViewModelTest
  {
    [Test]
    public void RemovePetriDishTest()
    {
      using (var container = new ComponentContainer())
      {
        // --arrange
        var target = container.CreateMainViewModel();
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
    }

    [Test]
    public void EventHandlerLeakTest()
    {
      using (var container = new ComponentContainer())
      {
        // --arrange
        var target = container.CreateMainViewModel();

        // --act
        target = null;

        // --assert
        dotMemory.Check(memory =>
        {
          Assert.That(
            memory.GetObjects(where => where.LeakedOnEventHandler()).ObjectsCount,
            Is.EqualTo(0), "Leaked");
        });
      }
    }
  }
}