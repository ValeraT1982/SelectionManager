using GalaSoft.MvvmLight;
using SelectionManager;

namespace SelectionManagerDemo.ViewModel
{
    class ListElementViewModel : ViewModelBase, ISelectableElement
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
    }
}
