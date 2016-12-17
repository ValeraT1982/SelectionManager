using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SelectionManager;

namespace SelectionManagerDemo.ViewModel
{
    class HierarchicalElementViewModel: ViewModelBase, ISelectableElement
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public ObservableCollection<HierarchicalElementViewModel> Subitems { get; set; }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }


        public ICommand AddSubitemCommand { get; }
        public ICommand RemoveCommand { get; }

        public HierarchicalElementViewModel ParentViewModel { get; }

        public HierarchicalElementViewModel(HierarchicalElementViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            Subitems = new ObservableCollection<HierarchicalElementViewModel>();
            AddSubitemCommand = new RelayCommand(Add);
            RemoveCommand = new RelayCommand(Remove, () => ParentViewModel != null);
        }

        private void Add()
        {
            Subitems.Add(new HierarchicalElementViewModel(this) { Name = "Child Element" });
        }
        private void Remove()
        {
            ParentViewModel.Subitems.Remove(this);
        }
    }
}
