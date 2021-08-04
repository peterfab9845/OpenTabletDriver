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
        public int InputReportLength => device.GetMaxInputReportLength(); // TODO
        public int OutputReportLength => device.GetMaxOutputReportLength(); // TODO
        public int FeatureReportLength => device.GetMaxFeatureReportLength(); // TODO

        public string Manufacturer => device.Descriptor.Manufacturer ?? "Unknown Manufacturer";
        public string ProductName => device.Descriptor.Product ?? "Unknown Product Name";
        public string FriendlyName => device.Descriptor.FullName ?? "Unknown Friendly Name";
        public string SerialNumber => device.Descriptor.SerialNumber ?? string.Empty;
        public string DevicePath => device.Descriptor.PathName ?? "Invalid Device Path";
        public bool CanOpen => true; // if a USBDevice exists then we can and have opened the device

        public IDeviceEndpointStream Open() => null; // TODO

        public string GetDeviceString(byte index)
        {
            // requestType = 0x80: IN direction, standard type, device recipient
            // request = 0x06: GET_DESCRIPTOR
            // value = 0x03XX: string descriptor type = 0x03, index 0xXX
            // index = 0x0000: no language ID
            // length = 0x00FF: request 255 bytes of descriptor
            byte[] strDesc = device.ControlIn(0x80, 0x06, 0x0300 | index, 0x0000, 0x00FF);
            int strLen = strDesc[0] - 2;
            return Encoding.Unicode.GetString(strDesc, 2, strLen);
        }
    }
}