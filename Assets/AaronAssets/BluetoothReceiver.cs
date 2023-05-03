using UnityEngine;
using System.IO.Ports;

public class BluetoothReceiver : MonoBehaviour
{
    public string portName = "COM3"; // change this to match the COM port your Bluetooth device is connected to
    public int baudRate = 9600; // change this to match the baud rate your Bluetooth device is running at
    private SerialPort serial;
    private string receivedData = "";
    private bool isConnected = false;

    public string ReceivedData {
        get {
            return receivedData;
        }
    }

    void Start()
    {
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 100;
        serial.Open();
        isConnected = true;
    }

    void Update()
    {
        if (isConnected)
        {
            try
            {
                string data = serial.ReadLine();
                receivedData = data;
                Debug.Log("Received data: " + data);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    void OnDisable()
    {
        isConnected = false;
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
        }
    }
}
