using System.Collections.ObjectModel;
using System.ComponentModel;
using NUnit.Framework;

namespace SelectionManager.Tests
{
    [TestFixture]
    public class SelectionManagerTests
    {
        private class SelectableElementStub : ISelectableElement
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public bool Selected
            {
                get { return _selected; }
                set
                {
                    _selected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selected"));
                }
            }

            public ObservableCollectionEx<SelectableElementStub> Subitems { get; }

            public ObservableCollection<string> Descriptions
            {
                get { return _descriptions; }

                set
                {
                    _descriptions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Descriptions"));
                }
            }

            private bool _selected;
            private ObservableCollection<string> _descriptions;

            public SelectableElementStub()
            {
                Subitems = new ObservableCollectionEx<SelectableElementStub>();
            }
        }

        [Test]
        public void EmptyCollectionTest()
        {
            var selectionManager = new SelectionManager();

            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>());

            Assert.IsNull(selectionManager.SelectedElement);
        }

        [Test]
        public void SelectElementTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            });

            element1.Selected = true;

            Assert.AreEqual(element1, selectionManager.SelectedElement);
        }

        [Test]
        public void SelectAnotherElementTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            });
            element1.Selected = true;
            element2.Selected = true;

            Assert.AreEqual(element2, selectionManager.SelectedElement);
            Assert.IsFalse(element1.Selected);
        }

        [Test]
        public void SelectTreeElementTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var element3 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            });
            element1.Subitems.Add(element3);

            element3.Selected = true;

            Assert.AreEqual(element3, selectionManager.SelectedElement);
        }

        [Test]
        public void SelectAnotherElementInTreeTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var element3 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            });
            element1.Subitems.Add(element3);
            element3.Selected = true;

            element2.Selected = true;

            Assert.AreEqual(element2, selectionManager.SelectedElement);
            Assert.IsFalse(element3.Selected);
        }

        [Test]
        public void TwoCollectionsTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var element3 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            });
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element3,
            });
            element1.Selected = true;

            element3.Selected = true;

            Assert.AreEqual(element3, selectionManager.SelectedElement);
            Assert.IsFalse(element1.Selected);
        }

        [Test]
        public void RemoveCollectionTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            var collection = new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            };
            selectionManager.AddCollection(collection);
            element1.Selected = true;
            selectionManager.RemoveCollection(collection);

            element2.Selected = true;

            Assert.IsNull(selectionManager.SelectedElement);
            Assert.IsTrue(element1.Selected);
            Assert.IsTrue(element2.Selected);
        }

        [Test]
        public void RemoveSelectedElementTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            var collection = new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            };
            selectionManager.AddCollection(collection);
            element2.Selected = true;
            collection.Remove(element2);

            Assert.AreEqual(element1, selectionManager.SelectedElement);
            Assert.IsTrue(element1.Selected);
        }

        [Test]
        public void RemoveSubitemsTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var element3 = new SelectableElementStub();
            element1.Subitems.Add(element2);
            element2.Subitems.Add(element3);
            var selectionManager = new SelectionManager();
            var collection = new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
            };
            selectionManager.AddCollection(collection);
            element3.Selected = true;
            element1.Subitems.Remove(element2);

            Assert.AreEqual(element1, selectionManager.SelectedElement);
        }

        [Test]
        public void ChangeAnotherPropertyTest()
        {
            var element1 = new SelectableElementStub();
            var element2 = new SelectableElementStub();
            var selectionManager = new SelectionManager();
            selectionManager.AddCollection(new ObservableCollectionEx<SelectableElementStub>
            {
                element1,
                element2
            });
            element2.Selected = true;

            element1.Descriptions = new ObservableCollection<string>();
            element1.Descriptions.Add("Test");

            Assert.AreEqual(element2, selectionManager.SelectedElement);
        }
    }
}