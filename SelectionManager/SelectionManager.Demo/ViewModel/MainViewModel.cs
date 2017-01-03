using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SelectionManager;

namespace SelectionManagerDemo.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<HierarchicalElementViewModel> HierarchicalElements { get; }

        public ObservableCollection<ListElementViewModel> ListElements { get; }

        public RelayCommand AddHierarchicalElementCommand { get; }

        public RelayCommand RemoveHierarchicalElementCommand { get; }

        public RelayCommand AddListElementCommand { get; }

        public RelayCommand RemoveListElementCommand { get; }

        public ISelectionManager Manager { get; }

        public MainViewModel()
        {
            HierarchicalElements = new ObservableCollection<HierarchicalElementViewModel>();
            ListElements = new ObservableCollection<ListElementViewModel>();
            AddHierarchicalElementCommand = new RelayCommand(AddHierarchicalElement);
            RemoveHierarchicalElementCommand = new RelayCommand(
                RemoveHierarchicalElement,
                () => Manager.SelectedElement is HierarchicalElementViewModel);
            AddListElementCommand = new RelayCommand(AddListElement);
            RemoveListElementCommand = new RelayCommand(
                RemoveListElement,
                () => Manager.SelectedElement is ListElementViewModel);
            Manager = new SelectionManager.SelectionManager();
            Manager.PropertyChanged += ManagerOnPropertyChanged;
            Manager.AddCollection(HierarchicalElements);
            Manager.AddCollection(ListElements);
        }

        private void AddHierarchicalElement()
        {
            var selectedHierarchicalElement = Manager.SelectedElement as HierarchicalElementViewModel;
            if (selectedHierarchicalElement != null)
            {
                var newItem = new HierarchicalElementViewModel(selectedHierarchicalElement) { Name = "Child Element" };
                selectedHierarchicalElement.Subitems.Add(newItem);
                newItem.Selected = true;
            }
            else
            {
                var newItem = new HierarchicalElementViewModel(null) { Name = "Root Element" };
                HierarchicalElements.Add(newItem);
                newItem.Selected = true;
            }
        }

        private void RemoveHierarchicalElement()
        {
            var hierarchicalElement = Manager.SelectedElement as HierarchicalElementViewModel;

            if (hierarchicalElement?.ParentViewModel != null)
            {
                hierarchicalElement.ParentViewModel.Subitems.Remove(hierarchicalElement);
            }
            else
            {
                HierarchicalElements.Remove(hierarchicalElement);
            }
        }

        private void AddListElement()
        {
            var newItem = new ListElementViewModel { Description = "List Element" };
            ListElements.Add(newItem);
            newItem.Selected = true;
        }

        private void RemoveListElement()
        {
            ListElements.Remove((ListElementViewModel)Manager.SelectedElement);
        }
        private void ManagerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RemoveHierarchicalElementCommand.RaiseCanExecuteChanged();
            RemoveListElementCommand.RaiseCanExecuteChanged();
        }
    }
}
