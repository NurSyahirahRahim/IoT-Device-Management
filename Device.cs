using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
