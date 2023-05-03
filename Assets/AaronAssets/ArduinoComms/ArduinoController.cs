using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort arduinoPort;
    string lastMessage;
    public static int power, fireState, jumpState;

    void Start()
    {
        arduinoPort = new SerialPort("COM3", 9600); // Replace "COM3" with the actual port name of your Arduino
        arduinoPort.Open();
        arduinoPort.ReadTimeout = 100; // Set a read timeout in milliseconds

        lastMessage = "";
        jumpState = 0;
        fireState = 0;
        power = 0;
    }

    void Update()
    {
        try
        {
            string message = arduinoPort.ReadLine().Trim(); // Read a line of text from the serial port and trim any whitespace characters
            if (message != lastMessage) // Check if the message is different from the last message received
            {
                string[] tokens = message.Split(','); 
                if (tokens.Length == 3) // Check if the message has 3 tokens (jumpState, fireState, and power)
                {
                    int.TryParse(tokens[0], out jumpState); 
                    int.TryParse(tokens[1], out fireState);
                    int.TryParse(tokens[2], out power); 
                }
                lastMessage = message;
            }
        }
        catch (System.Exception)
        {
            // Handle any read errors (to be edited)
        }
        Debug.Log(jumpState+" "+fireState+" "+ power);
    }

    void OnDestroy()
    {
        if (arduinoPort != null && arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }
}
