using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort arduinoPort;

    void Start()
    {
        arduinoPort = new SerialPort("COM3", 9600); // Replace "COM3" with the actual port name of your Arduino
        arduinoPort.Open();
        arduinoPort.ReadTimeout = 100; // Set a read timeout in milliseconds
    }

    void Update()
    {
        try
        {
            string message = arduinoPort.ReadLine().Trim(); // Read a line of text from the serial port and trim any whitespace characters
            if (message != ManagerVar.Instance.lastTagID) // Check if the message is different from the last message received
            {
                if (message.Length == 8) // Check if the message is 8 characters long (RFID UID)
                {
                    ManagerVar.Instance.lastTagID = message;
                }
                else
                {
                    string[] tokens = message.Split(','); 
                    if (tokens.Length == 3) // Check if the message has 3 tokens (jumpState, fireState, and power)
                    {
                        int.TryParse(tokens[0], out ManagerVar.Instance.jumpState); 
                        int.TryParse(tokens[1], out ManagerVar.Instance.fireState);
                        int.TryParse(tokens[2], out ManagerVar.Instance.power); 
                    }
                }
            }
        }
        catch (System.Exception)
        {
            // Handle any read errors (to be edited)
        }
        Debug.Log(ManagerVar.Instance.lastTagID + " " + ManagerVar.Instance.jumpState + " " + ManagerVar.Instance.fireState + " " + ManagerVar.Instance.power);
    }

    void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }
}
