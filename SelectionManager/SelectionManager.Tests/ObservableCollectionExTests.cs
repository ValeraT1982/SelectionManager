using System.Collections.Specialized;
using NUnit.Framework;

namespace SelectionManager.Tests
{
    [TestFixture]
    class ObservableCollectionExTests
    {
        [Test]
        public void ClearTest()
        {
            var wasExecuted = false;
            var collection = new ObservableCollectionEx<string> {"One", "Two"};
            collection.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Remove && args.OldItems.Count == 2)
                {
                    wasExecuted = true;
                }
            };

            collection.Clear();

            CollectionAssert.IsEmpty(collection);
            Assert.IsTrue(wasExecuted);
        }
    }
}
