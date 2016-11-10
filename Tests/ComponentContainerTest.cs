using GameOfLife;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;

namespace Tests
{
    public class ComponentContainerTest
    {
        [Test]
        [DotMemoryUnit(CollectAllocations = true)]
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
                var allMyObjects = memory
                    .GetObjects(where => @where.Type.IsNot<ComponentContainer>())
                    .GetObjects(where => @where.Namespace.Like("GameOfLife.*"));

                Assert.That(allMyObjects.ObjectsCount, Is.EqualTo(1));

//        var allowedObject = allMyObjects.
            });

            dotMemoryApi.SaveCollectedData();
        }

    }
}