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
            RunAndShutdownApplication();

            // --assert
            dotMemory.Check(memory =>
            {
                Assert.That(memory.GetObjects(@where => @where.Namespace.Like("GameOfLife.*")).ObjectsCount, Is.EqualTo(0));
            });
        }

        /// <summary>
        /// When code is build in the 'debug' configuration CLR doesn't collect the object till exiting a visibility scope,
        /// even if it is not referenced, in contrast with 'release' configuration when object is collected as soon
        /// as it is not referenced by any root. 
        /// This method is need To eliminate this difference and make the test stable by isolating variable
        /// 'target' visibility scope.
        /// </summary>
        private static void RunAndShutdownApplication()
        {
            // --arrange
            var target = new ComponentContainer();
            target.CreateMainViewModel();

            // --act
            target.Dispose();
        }
    }
}