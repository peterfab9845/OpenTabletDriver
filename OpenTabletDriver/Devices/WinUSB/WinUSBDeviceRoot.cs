using System;
using System.Collections.Generic;
using WinUSBNet;
using OpenTabletDriver.Plugin.Devices;

namespace OpenTabletDriver.Devices.WinUSB
{
    public class WinUSBDeviceRoot : IRootHub
    {
        private WinUSBDeviceRoot()
        {

        }

        public static IRootHub Current { get; } = new WinUSBDeviceRoot();

        private IEnumerable<USBDeviceInfo> deviceInfos = USBDevice.GetDevices("{A5DCBF10-6530-11D2-901F-00C04FB951ED}");

        public event EventHandler<DevicesChangedEventArgs> DevicesChanged;

        public IEnumerable<IDeviceEndpoint> GetDevices()
        {
            ICollection<USBDevice> openDevices = new List<USBDevice>();
            foreach(USBDeviceInfo info in deviceInfos)
            {
                try
                {
                    openDevices.Add(new USBDevice(info));
                }
                catch (USBException e) when (e.Message.Contains("Failed to retrieve device descriptor."))
                {
                    // not able to open the device, often expected.
                    // ideally would be able to filter on WinUSB drivers, but needs custom INF file etc.
                }
            }
            return openDevices;
        }
    }
}