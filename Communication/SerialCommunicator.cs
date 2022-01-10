using System;
using System.IO.Ports;

namespace MediaPanelController.Communication
{
    internal class SerialCommunicator
    {
        private SerialPort serialPort = null;

        private readonly string serialDeviceName;
        private readonly int serialBaudRate = 115200;
        private readonly Parity serialParity = Parity.None;
        private readonly int serialDataBits = 8;
        private readonly StopBits serialStopBits = StopBits.One;
        private readonly int serialRepeatOnError = 1;

        private string lastMessage = "";

        private SerialMessageContainer messageQueue = null;
        internal SerialCommunicator(string serialDeviceName)
        {
            this.serialDeviceName = serialDeviceName;

            messageQueue = new SerialMessageContainer();
            messageQueue.MessageIncoming += WriteToSerialDevice;

            seekSerialDeviceByName();
        }

        private void Connect(String portName)
        {
            try
            {
                serialPort = new SerialPort(portName, serialBaudRate, serialParity, serialDataBits, serialStopBits);
                serialPort.ReadTimeout = 250;
                serialPort.DtrEnable = true;
                serialPort.Open();
            }
            catch (InvalidOperationException)
            {
                serialPort = null;
            }
        }

        private void Disconnect()
        {
            try
            {
                if(serialPort != null)
                {
                    serialPort.Close();
                }
            }
            finally
            {
                serialPort = null;
            }
        }

        private bool IsConnected()
        {
            return serialPort != null && serialPort.IsOpen;
        }

        private void seekSerialDeviceByName()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                try
                {
                    Connect(portName);
                    serialPort.Write("whoru");
                    String data = serialPort.ReadLine();
                    if (data.Trim().Equals(serialDeviceName))
                    {
                        break;
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        public void Write(string serialMessage)
        {
            lock (messageQueue.SyncRoot)
            {
                messageQueue.Add(serialMessage);
            }
        }

        private void WriteToSerialDevice()
        {
            lock (messageQueue.SyncRoot)
            {
                string serialMessage = (string)messageQueue.Shift();
                if (!lastMessage.Equals(serialMessage))
                {
                    for (int i = 0; i < serialRepeatOnError; ++i)
                    {
                        if (!IsConnected())
                        {
                            seekSerialDeviceByName();
                        }

                        if (IsConnected())
                        {
                            try
                            {
                                serialPort.Write(serialMessage);
                                lastMessage = serialMessage;
                                break;
                            }
                            catch
                            {
                                Disconnect();
                            }
                        }
                    }
                }
            }
        }
    }
}
