using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json;


namespace IoT_Device_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Device> Devices { get; set; }
        private int nextDeviceId = 1;
        private readonly string filePath = "devices.json";
        private DispatcherTimer simulationTimer;
        private Random random = new Random();
        private List<Device> editedDevices = new List<Device>();
        private List<Device> addedDevices = new List<Device>();
        private List<Device> deletedDevices = new List<Device>();


        public MainWindow()
        {
            InitializeComponent();
            // Auto load devices from file if available
            Devices = LoadDevicesFromFile();
            DeviceGrid.ItemsSource = Devices;

            if (Devices.Count > 0)
                nextDeviceId = Devices[^1].DeviceID + 1;

            this.Closing += MainWindow_Closing;

            // Start device simulation
            StartDeviceSimulation();
        }

        private void StartDeviceSimulation()
        {
            simulationTimer = new DispatcherTimer();
            simulationTimer.Interval = TimeSpan.FromSeconds(120); // change every 120 sec
            simulationTimer.Tick += SimulationTimer_Tick;
            simulationTimer.Start();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            if (Devices.Count == 0) return;

            // Randomly pick 1-2 devices to toggle status
            int devicesToToggle = random.Next(1, Math.Min(3, Devices.Count + 1));
            for (int i = 0; i < devicesToToggle; i++)
            {
                var device = Devices[random.Next(Devices.Count)];

                // 50% chance to change status
                if (random.NextDouble() > 0.5)
                {
                    device.Status = device.Status == "Online" ? "Offline" : "Online";
                    device.LastActive = DateTime.Now;

                    LogAction($"Toggle Status: {device.DeviceName} ({device.Status})");

                    if (device.Status == "Offline")
                    {
                        LogAction($"ERROR: Device '{device.DeviceName}' is disconnected.");
                    }
                }
            }

            // Refresh DataGrid
            DeviceGrid.Items.Refresh();
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
                new Device { DeviceID = 1, DeviceName = "Smart Light", DeviceType = "Smart Light", Status = "Online", LastActive = DateTime.Now.AddMinutes(-5)},
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

        private void ButtonAddDevice_Click(object sender, RoutedEventArgs e)
        {
            string name = txtDeviceName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a device name.");
                return;
            }

            string deviceType = (cmbDeviceType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Smart Device";

            var newDevice = new Device
            {
                DeviceID = nextDeviceId++,
                DeviceName = name,
                DeviceType = deviceType,
                Status = "Online",
                LastActive = DateTime.Now
            };

            Devices.Add(newDevice);
            txtDeviceName.Clear();
            cmbDeviceType.SelectedIndex = -1;
            addedDevices.Add(newDevice);
        }

        private void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Device device)
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{device.DeviceName}'?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    deletedDevices.Add(device);
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

                // Log any added devices
                foreach (var device in addedDevices)
                {
                    Console.WriteLine(device);
                    LogAction($"Add device: {device.DeviceName} ({device.DeviceType})");
                }

                // Log any edited devices
                foreach (var device in editedDevices)
                {
                    Console.WriteLine(device);
                    LogAction($"Update device: {device.DeviceName} ({device.DeviceType})");
                }

                // Log any deleted devices
                foreach (var device in deletedDevices)
                {
                    Console.WriteLine(device);
                    LogAction($"Delete device: {device.DeviceName} ({device.DeviceType})");
                }

                addedDevices.Clear();
                editedDevices.Clear();
                deletedDevices.Clear();

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
                
                editedDevices.Add(device);
            }
        }

        private void LogAction (string message)
        {
            string logEntry = $"{DateTime.Now:HH:mm:ss} - {message}";
            ActionLog.Items.Insert(0, logEntry);
        }
    }
}
