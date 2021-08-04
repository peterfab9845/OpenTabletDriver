using System.IO;
using WinUSBNet;
using OpenTabletDriver.Plugin.Devices;

namespace OpenTabletDriver.Devices.WinUSB
{
    public class WinUSBEndpointStream : IDeviceEndpointStream
    {
        // TODO: entire class. interfaces -> pipes or just pipes from device? multiples?
        internal WinUSBEndpointStream(HidStream stream)
        {
            this.stream = stream;
            stream.ReadTimeout = int.MaxValue;
        }

        private HidStream stream;
        Stream IDeviceEndpointStream.Stream => stream;

        public byte[] Read() => stream.Read();
        public void Write(byte[] buffer) => stream.Write(buffer);

        public void GetFeature(byte[] buffer) => stream.GetFeature(buffer);
        public void SetFeature(byte[] buffer) => stream.SetFeature(buffer);

        public void Dispose() => stream.Dispose();
    }
}