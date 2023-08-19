using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UDPAudioReceiver : MonoBehaviour
{
    UdpClient client;
    int dataPort = 5001; // Audio Streaming Port

    Thread receiveThread;
    AudioClip audioClip;
    AudioSource audioSource;

    void Start()
    {
        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Please attach an AudioSource component to this GameObject.");
            return;
        }

        // Create UDP client
        client = new UdpClient(dataPort);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), dataPort);
        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref anyIP);

                // Decode audio data
                float[] audioData = new float[data.Length / 2];
                for (int i = 0; i < audioData.Length; i++)
                {
                    audioData[i] = System.BitConverter.ToInt16(data, i * 2) / 32768.0f;
                }

                // Create AudioClip on main thread
                MainThreadDispatcher.RunOnMainThread(() =>
                {
                    audioClip = AudioClip.Create("ReceivedAudio", audioData.Length, 1, 44100, false);
                    audioClip.SetData(audioData, 0);

                    // Play AudioClip
                    audioSource.clip = audioClip;
                    audioSource.Play();
                });

            }
            catch (SocketException ex)
            {
                Debug.Log("UDP Receive Failed " + ex);
                break;
            }
        }
    }


    private void OnDestroy()
    {
        receiveThread.Abort();
        client.Close();
    }
}
