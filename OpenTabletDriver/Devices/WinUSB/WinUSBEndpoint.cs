using System.Text;
using WinUSBNet;
using OpenTabletDriver.Plugin.Devices;

namespace OpenTabletDriver.Devices.WinUSB
{
    public class WinUSBEndpoint : IDeviceEndpoint
    {
        internal WinUSBEndpoint(USBDevice device)
        {
            this.device = device;
        }

        private USBDevice device;

        public int ProductID => device.Descriptor.PID;
        public int VendorID => device.Descriptor.VID;
        public int InputReportLength => device.GetMaxInputReportLength();
        public int OutputReportLength => device.GetMaxOutputReportLength();
        public int FeatureReportLength => device.GetMaxFeatureReportLength();

        public string Manufacturer => device.Descriptor.Manufacturer ?? "Unknown Manufacturer";
        public string ProductName => device.Descriptor.Product ?? "Unknown Product Name";
        public string FriendlyName => device.Descriptor.FullName ?? "Unknown Friendly Name";
        public string SerialNumber => device.Descriptor.SerialNumber ?? string.Empty;
        public string DevicePath => device.Descriptor.PathName ?? "Invalid Device Path";
        public bool CanOpen => true;

        public IDeviceEndpointStream Open() => device.TryOpen(out var stream) ? new WinUSBEndpointStream(stream) : null;

        public string GetDeviceString(byte index)
        { 
            byte[] strDesc = device.ControlIn(0x80, 0x06, 0x0300 | index, 0x0000, 0x00FF);
            int strLen = strDesc[0] - 2;
            return Encoding.Unicode.GetString(strDesc, 2, strLen);
        }
    }
}