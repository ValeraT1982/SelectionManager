using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SelectionManager
{
    /// <summary>
    /// Works the same as <see cref="ObservableCollection{T}"/>. 
    /// Fires <see cref="ObservableCollection{T}.CollectionChanged"/> event with <see cref="NotifyCollectionChangedEventArgs.Action"/> equal to <see cref="NotifyCollectionChangedAction.Remove"/> after calling <see cref="Collection{T}.Clear"/> methods.
    /// <see cref="ObservableCollection{T}"/> fires event with <see cref="NotifyCollectionChangedEventArgs.Action"/> equal to <see cref="NotifyCollectionChangedAction.Reset"/> and empty list of old items./>
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Removes all items from the collection and fire CollectionChanged
        /// </summary>
        protected override void ClearItems()
        {
            var items = new List<T>(Items);
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
        }
    }
}
