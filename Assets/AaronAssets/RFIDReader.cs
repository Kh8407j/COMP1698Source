using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class RFIDReader : MonoBehaviour
{
    private SerialPort serialPort = new SerialPort("COM3", 9600);  // Change the COM port to match your Arduino

    void Start()
    {
        serialPort.Open();
        serialPort.ReadTimeout = 100;
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string tagID = serialPort.ReadLine().Trim();
                Debug.Log(tagID);
            }
            catch (System.Exception)
            {
                // Ignore exceptions caused by timeouts
            }
        }
    }

    void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
