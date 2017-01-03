using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SelectionManager
{
    public class SelectionManager : ISelectionManager
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets and sets selected element
        /// </summary>
        public ISelectableElement SelectedElement
        {
            get
            {
                return _selectedElement;
            }

            set
            {
                _selectedElement = value;
                OnPropertyChanged();
            }
        }

        private ISelectableElement _selectedElement;
        private readonly List<ISelectableElement> _elements = new List<ISelectableElement>();

        /// <summary>
        /// Adds collection of the objects to manager
        /// </summary>
        /// <param name="collection">The collection to be added</param>
        public void AddCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += collection_CollectionChanged;
            foreach (var element in (ICollection)collection)
            {
                var selectableElement = element as ISelectableElement;
                if (selectableElement != null)
                {
                    AddElement(selectableElement);
                }
            }
        }

        /// <summary>
        /// Removes collection of the objects from manager
        /// </summary>
        /// <param name="collection">The collection to be removed</param>
        public void RemoveCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= collection_CollectionChanged;
            foreach (var element in (ICollection)collection)
            {
                var selectableElement = element as ISelectableElement;
                if (selectableElement != null)
                {
                    RemoveElement(selectableElement);
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void AddElement(ISelectableElement element)
        {
            _elements.Add(element);
            element.PropertyChanged += element_PropertyChanged;
            AddSelectableElements(element);
            if (_elements.Any() && _elements.All(e => !e.Selected))
            {
                _elements[0].Selected = true;
            }
        }

        private void RemoveElement(ISelectableElement element)
        {
            _elements.Remove(element);
            RemoveSelectableElements(element);
            element.PropertyChanged -= element_PropertyChanged;

            if (SelectedElement == element)
            {
                SelectedElement = null;
                if (_elements.Count > 0)
                {
                    _elements[0].Selected = true;
                }
            }
        }

        private void element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var currentElement = (ISelectableElement)sender;
            if (e.PropertyName != PropertyHelper.GetPropertyName(() => currentElement.Selected))
            {
                return;
            }

            if (currentElement.Selected)
            {
                foreach (var selectedElement in _elements.Where(element => element != currentElement && element.Selected))
                {
                    selectedElement.Selected = false;
                }

                SelectedElement = currentElement;
            }
            else
            {
                SelectedElement = null;
            }
        }

        private void collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (e.OldItems == null || !e.OldItems.Contains(item))
                    {
                        var element = item as ISelectableElement;
                        if (element != null)
                        {
                            AddElement(element);
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (e.NewItems == null || !e.NewItems.Contains(item))
                    {
                        var element = item as ISelectableElement;
                        if (element != null)
                        {
                            RemoveElement(element);
                        }
                    }
                }
            }
        }

        private void AddSelectableElements(ISelectableElement rootElement)
        {
            foreach (var prop in rootElement.GetType().GetProperties().Where(IsPropertyObservable))
            {
                var value = (INotifyCollectionChanged)prop.GetValue(rootElement);
                AddCollection(value);
            }
        }

        private void RemoveSelectableElements(ISelectableElement rootElement)
        {
            foreach (var prop in rootElement.GetType().GetProperties().Where(IsPropertyObservable))
            {
                var value = (INotifyCollectionChanged)prop.GetValue(rootElement);
                RemoveCollection(value);
            }
        }

        private bool IsPropertyObservable(PropertyInfo prop)
        {
            if (!prop.PropertyType.IsGenericType)
            {
                return false;
            }

            var observableCollectionType = GetObservableCollectionType(prop.PropertyType);
            if (observableCollectionType != null &&
                typeof(ISelectableElement).IsAssignableFrom(observableCollectionType.GenericTypeArguments[0]))
            {
                return true;
            }

            return false;
        }

        private Type GetObservableCollectionType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                return type;
            }

            if (type.BaseType == null)
            {
                return null;
            }

            return GetObservableCollectionType(type.BaseType);
        }
    }
}
