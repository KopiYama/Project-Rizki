using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class BluetoothManager : MonoBehaviour
{
    public string deviceName = "JDY-31-SPP"; // Bluetooth module device name
    public string result = "00112";

    private SerialPort serialPort;
    private string portName;

    void Start()
    {
        portName = FindBluetoothPort(deviceName);
        
        if (portName != null)
        {
            serialPort = new SerialPort(portName, 9600);
            serialPort.ReadTimeout = 100;
            serialPort.Open();

            if (serialPort.IsOpen)
            {
                Debug.Log("Connected to " + deviceName);
                SendMessageToArduino(result);
            }
        }
        else
        {
            Debug.LogError("Device not found");
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    private string FindBluetoothPort(string targetDeviceName)
    {
        string[] portNames = SerialPort.GetPortNames();

        foreach (string port in portNames)
        {
            try
            {
                Debug.Log(port);
                SerialPort tempPort = new SerialPort(port, 9600);
                tempPort.ReadTimeout = 100;
                tempPort.Open();
                tempPort.WriteLine("AT+NAME?");
                Debug.Log('a');
                string response = tempPort.ReadLine();

                if (response.Contains(targetDeviceName))
                {
                    tempPort.Close();
                    return port;
                }

                tempPort.Close();
            }
            catch (System.Exception) { }
        }

        return null;
    }

    public void SendMessageToArduino(string message)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.WriteLine(message);
            Debug.Log("Sent: " + message);
        }
        else
        {
            Debug.LogError("Unable to send message. Check if Bluetooth is connected.");
        }
    }
}
