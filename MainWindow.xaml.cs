using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace IoT_Device_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Device> Devices { get; set; }
        private int nextDeviceId = 1; // 
        private readonly string filePath = "devices.json";
        public MainWindow()
        {
            InitializeComponent();
            // Auto load devices from file if available
            Devices = LoadDevicesFromFile();
            DeviceGrid.ItemsSource = Devices;
        }

        private ObservableCollection<Device> LoadDevicesFromFile()
        {
            // Load from file if exists
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<ObservableCollection<Device>>(json)
                           ?? new ObservableCollection<Device>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading devices: " + ex.Message);
                }
            }

            return new ObservableCollection<Device>
            {
                new Device { DeviceID = 1, DeviceName = "Smart Light 1", DeviceType = "Smart Light", Status = "Online", LastActive = DateTime.Now.AddMinutes(-5)},
                new Device { DeviceID = 2, DeviceName = "Living Room Camera", DeviceType = "Security Camera", Status = "Offline", LastActive = DateTime.Now.AddHours(-12)},
                new Device { DeviceID = 3, DeviceName = "Thermostat", DeviceType = "Temperature Sensor", Status = "Online", LastActive = DateTime.Now.AddMinutes(-10)},
                new Device { DeviceID = 4, DeviceName = "Door Lock", DeviceType = "Smart Lock", Status = "Online", LastActive = DateTime.Now.AddMinutes(-20)},
                new Device { DeviceID = 5, DeviceName = "Garage Sensor", DeviceType = "Motion Sensor", Status = "Offline", LastActive = DateTime.Now.AddDays(-2)},
                new Device { DeviceID = 6, DeviceName = "Kitchen Plug", DeviceType = "Smart Plug", Status = "Online", LastActive = DateTime.Now.AddMinutes(-2)}
            };
        }

        // Auto-Save on Close
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDevicesToFile();
        }

        // Save Devices to File
        private void SaveDevicesToFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Devices, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving devices: " + ex.Message);
            }
        }

        private void ToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Device device)
            {
                device.Status = device.Status == "Online" ? "Offline" : "Online";
                device.LastActive = DateTime.Now;
                DeviceGrid.Items.Refresh();
            }
        }

        private void ButtonAddDevice_Click(object sender, RoutedEventArgs e)
        {
            string name = txtDeviceName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a device name.");
                return;
            }

            string deviceType = (cmbDeviceType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Smart Device";

            Devices.Add(new Device
            {
                DeviceID = nextDeviceId++,
                DeviceName = name,
                DeviceType = deviceType,
                Status = "Online",
                LastActive = DateTime.Now
            });

            txtDeviceName.Clear();
            cmbDeviceType.SelectedIndex = -1;
        }

        private void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Device device)
            {
                // Optional confirmation
                var result = MessageBox.Show($"Are you sure you want to delete '{device.DeviceName}'?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Devices.Remove(device);
                }
            }
        }

        private void SaveDevices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Devices, Formatting.Indented);
                File.WriteAllText(filePath, json);
                MessageBox.Show("Devices saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving devices: " + ex.Message);
            }
        }

        // Update LastActive when edited inline
        private void DeviceGrid_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is Device device)
            {
                device.LastActive = DateTime.Now;
                DeviceGrid.Items.Refresh();
            }
        }
    }
}