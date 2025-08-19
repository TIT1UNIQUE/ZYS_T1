using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort;
    public string portName = "COM7"; // ����Arduino�˿��޸�
    public int baudRate = 9600;
    public string dataString;


    private float distance;

    void Start()
    {
        RestartPort();
    }

    public void SetPort(string s, int br)
    {
        portName = s;
        baudRate = br;
    }

    public void RestartPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }

        //��ʼ���˿�
        serialPort = new SerialPort(portName, baudRate);
        //���ó�ʱʱ��
        serialPort.ReadTimeout = 1000;
        //���ļ�
        serialPort.Open();
    }

    void Update()
    {
        //���ж�ȡ����
        if (serialPort.BytesToRead > 0)
            dataString = serialPort.ReadLine();


        //ת��ʽ
        if (!string.IsNullOrEmpty(dataString))
        {
            //distance: 60.27 cm
            // string afterColon = dataString.Split(':')[1];
            //   Debug.Log(afterColon);
            // 2. Get the first token that can be parsed as a float
            //   string cleaned = afterColon.Replace("cm", "").Trim();
            //  Debug.Log(cleaned);
            // 3. Convert safely
            //  float.TryParse(cleaned, out distance);


            float.TryParse(dataString, out distance);
        }

    }

    //��ȡ�������
    public float GetDistance()
    {
        return distance;
    }
}

