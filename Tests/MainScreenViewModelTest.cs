using GameOfLife.ViewModel;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace Tests
{
    public class MainScreenViewModelTest
    {
        [Test]
        public void RemovePetriDish()
        {
            // --arrange
            using (var target = new MainScreenViewModel(0))
            {
                target.AddPetriDishCommand.Execute(null);

                // --act
                target.RemovePetriDishCommand.Execute(null);

                // --assert
                Assert.That(target.PetriDishesCollection, Has.Count.EqualTo(0));

                // --assert memory
                dotMemory.Check(memory =>
                    Assert.That(memory.GetObjects(where => where.
                                Type.Is<PetriDish>())
                            .ObjectsCount,
                        Is.EqualTo(0)));
            }
        }

        [Test]
        public void NoObjectsLeakedOnEventHandler()
        {
            // --act
            CreateAndReleaseMainScreenViewModel();

            // --assert
            dotMemory.Check(memory =>
                Assert.That(memory.GetObjects(where => where.
                            LeakedOnEventHandler())
                        .ObjectsCount,
                    Is.EqualTo(0)));
        }

        [Test]
        public void AllObjectsAreReleased()
        {
            // --act
            CreateAndReleaseMainScreenViewModel();

            // --assert
            dotMemory.Check(memory =>
                Assert.That(
                    memory.GetObjects(where => where.
                            Namespace.Like("GameOfLife.*"))
                        .ObjectsCount,
                    Is.EqualTo(0)));
        }

        private static void CreateAndReleaseMainScreenViewModel()
        {
            using (new MainScreenViewModel(2))
            {
            }
        }
    }
}