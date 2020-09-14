using System;
using System.Threading;
using System.Timers;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Properties;
using NUnit.Framework;
using Production;
using Timer = System.Timers.Timer;

namespace Production
{
    public interface IShape
    {
    }

    public class Triangle : IShape
    {
        public static readonly Triangle Instance = new Triangle();
    }

    public class Rectangle : IShape
    {
        public static readonly Rectangle Instance = new Rectangle();
    }

    public class Circle : IShape
    {
        public static readonly Circle Instance = new Circle();
    }

    public class ShapeGenerator : IDisposable
    {
        private readonly Action<IShape> consumer;
        private readonly Timer timer;

        public ShapeGenerator(Action<IShape> consumer, TimeSpan frequency)
        {
            this.consumer = consumer;
            timer = new Timer(frequency.TotalMilliseconds);
            timer.Elapsed += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object state, ElapsedEventArgs elapsedEventArgs)
        {
            var rand = DateTime.UtcNow.Ticks % 3;
            switch (rand)
            {
                case 0:
                    consumer(new Triangle());
                    // consumer(Triangle.Instance); // change to fix TrafficTest
                    break;
                case 1:
                    consumer(new Rectangle());
                    // consumer(Rectangle.Instance); // change to fix TrafficTest
                    break;
                case 2:
                    consumer(new Circle());
                    // consumer(Circle.Instance); // change to fix TrafficTest
                    break;
            }
        }

        public void Dispose()
        {
            timer.Elapsed -= OnTimerTick;
        }
    }
}

namespace Tests
{
    public class SimplestDemoTest
    {
        [Test]
        public void LeakTest()
        {
            var isolator = new Action(() =>
            {
                using (new ShapeGenerator(_ => { }, TimeSpan.FromMilliseconds(100)))
                {
                    Thread.Sleep(300);
                }
            });

            isolator();

            dotMemory.Check(memory =>
                Assert.That(memory
                        .GetObjects(where => where.Type.Is<ShapeGenerator>())
                        .ObjectsCount,
                    Is.EqualTo(0)));
        }

        [AssertTraffic(AllocatedObjectsCount = 3, Interfaces = new[] {typeof(IShape)})]
        [Test(Description = "Should use singletons and should not produce more than 3 shapes")]
        public void TrafficTest()
        {
            using (new ShapeGenerator(_ => { }, TimeSpan.FromMilliseconds(100)))
            {
                Thread.Sleep(1000); // generate ~10 shapes
            }
        }

        [Test]
        public void MiddleAgeTest()
        {
            using (new ShapeGenerator(_ => { }, TimeSpan.FromMilliseconds(100)))
            {
                Thread.Sleep(1000); // generate ~10 shapes

                dotMemory.Check(memory =>
                    {
                        var objectSet = memory
                            .GetObjects(where => where.Namespace.Like("Production"))
                            .GetObjects(where => where.Generation.Is(Generation.Gen2));

                        Assert.That(objectSet.ObjectsCount, Is.EqualTo(0));
                    }
                );
            }
        }

        [Test]
        public void NoNewObjectsExceptShapesTest()
        {
            using (new ShapeGenerator(_ => { }, TimeSpan.FromMilliseconds(100)))
            {
                var memoryCheckPoint = dotMemory.Check();
                Thread.Sleep(1000); // generate ~10 shapes

                dotMemory.Check(memory =>
                {
                    var newTotalCount =
                        memory
                            .GetDifference(memoryCheckPoint)
                            .GetNewObjects(where => where.Namespace.Like("Production"))
                            .ObjectsCount;

                    var newShapesCount =
                        memory
                            .GetDifference(memoryCheckPoint)
                            .GetNewObjects(where => where.Interface.Is<IShape>())
                            .ObjectsCount;

                    Assert.That(newTotalCount - newShapesCount, Is.EqualTo(0));
                });
            }
        }
    }
}