using System.Collections.Specialized;
using System.ComponentModel;

namespace SelectionManager
{
    /// <summary>
    /// Manages SelectedElement in hierarchical collection of elements (only one element selected at the particular moment).
    /// </summary>
    public interface ISelectionManager: INotifyPropertyChanged
    {
        /// <summary>
        /// Gets and sets selected element
        /// </summary>
        ISelectableElement SelectedElement { get; set; }

        /// <summary>
        /// Adds collection of the objects to manager
        /// </summary>
        /// <param name="collection">The collection to be added</param>
        void AddCollection(INotifyCollectionChanged collection);

        /// <summary>
        /// Removes collection of the objects from manager
        /// </summary>
        /// <param name="collection">The collection to be removed</param>
        void RemoveCollection(INotifyCollectionChanged collection);
    }
}
