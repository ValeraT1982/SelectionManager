using System.ComponentModel;

namespace SelectionManager
{
    /// <summary>
    /// Classes must implement this interface to be handled by <see cref="SelectionManager"/>
    /// <remarks>Property <see cref="Selected"/> have to fire PropertyChanged event./></remarks>
    /// </summary>
    public interface ISelectableElement: INotifyPropertyChanged
    {
        /// <summary>
        /// Selection flag.
        /// </summary>
        bool Selected { get; set; }
    }
}