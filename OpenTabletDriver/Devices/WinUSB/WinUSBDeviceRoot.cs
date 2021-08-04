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
            // TODO: notifier OnArrival, OnRemoval -> DevicesChanged
        }

        public static IRootHub Current { get; } = new WinUSBDeviceRoot();

        private Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("{A5DCBF10-6530-11D2-901F-00C04FB951ED}");

        private IEnumerable<USBDeviceInfo> deviceInfos = USBDevice.GetDevices(GUID_DEVINTERFACE_USB_DEVICE);

        private USBNotifier notifier = new USBNotifier(0 /* TODO: window handle */, GUID_DEVINTERFACE_USB_DEVICE);

        public event EventHandler<DevicesChangedEventArgs> DevicesChanged; // TODO

        public IEnumerable<IDeviceEndpoint> GetDevices()
        {
            // WinUSBNet doesn't provide CanOpen, so just try all available
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
                    // ideally would be able to filter for WinUSB drivers, but needs custom INF file etc.
                }
            }
            return openDevices;
        }
    }
}