using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UDPReceiver : MonoBehaviour
{
    public RawImage LeftDisplay;
    public RawImage RightDisplay;

    private Socket _socket;
    private EndPoint _endPoint;
    private byte[] _buffer;

    private void Start()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _endPoint = new IPEndPoint(IPAddress.Any, 5000);
        _socket.Bind(_endPoint);
        _buffer = new byte[65507]; // Maximum UDP packet size
    }

    private void Update()
    {
        while (_socket.Available > 0)
        {
            var received = _socket.ReceiveFrom(_buffer, ref _endPoint);
            var data = new byte[received];
            Array.Copy(_buffer, data, received);
            var tex = new Texture2D(1, 1);
            tex.LoadImage(data);

            ImageProcesses.FlipTextureHorizontally(tex);

            SetTextureWithCorrectRatio(tex);
            //LeftDisplay.texture = tex;
            //RightDisplay.texture = tex;
            
        }
    }

    private void SetTextureWithCorrectRatio(Texture2D tex)
    {
        float aspectRatio = (float)tex.width / (float)tex.height;

        LeftDisplay.texture = tex;
        RightDisplay.texture = tex;
        LeftDisplay.rectTransform.sizeDelta = new Vector2(LeftDisplay.rectTransform.sizeDelta.y * aspectRatio, LeftDisplay.rectTransform.sizeDelta.y);
        RightDisplay.rectTransform.sizeDelta = new Vector2(RightDisplay.rectTransform.sizeDelta.y * aspectRatio, RightDisplay.rectTransform.sizeDelta.y);

        LeftDisplay.SetNativeSize();
        RightDisplay.SetNativeSize();
    }
}