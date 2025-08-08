using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace IoT_Device_Management
{
    public class Device
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string Status { get; set; }
        public DateTime LastActive { get; set; }
    }
}

public class ItemVM : IEditableObject, INotifyPropertyChanged
{
    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    void IEditableObject.BeginEdit()
    {
        throw new NotImplementedException();
    }

    void IEditableObject.CancelEdit()
    {
        throw new NotImplementedException();
    }

    void IEditableObject.EndEdit()
    {
        throw new NotImplementedException();
    }
}


public class ModuleVM : INotifyPropertyChanged
{
    ICollectionView Items { get; }

    public ModuleVM(ObservableCollection<ItemVM> items)
    {
        Items = CollectionViewSource.GetDefaultView(items);
    }

    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    public void RefreshSafely()
    {
        ((IEditableCollectionView)Items).CommitEdit(); 
        Items.Refresh();
    }
}
