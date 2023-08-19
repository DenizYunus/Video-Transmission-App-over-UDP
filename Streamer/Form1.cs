using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using NAudio.Codecs;
using NAudio.Wave;
using System.Net.Sockets;

namespace Freelance_VideoTransmissionApp
{
    public partial class Form1 : Form
    {
        int videoStreamingPort = 5000;
        int audioStreamingPort = 5001;

        private VideoCaptureDevice videoSource;
        private FilterInfoCollection videoDevices;
        private UdpClient udpVideoSender;
        private UdpClient udpVideoReceiver;

        private WaveInEvent waveSource;
        private UdpClient udpAudioSender;
        private UdpClient udpAudioReceiver;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;

        VideoForm videoForm;

        private byte[] endOfFrameMarker = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };

        public Form1()
        {
            InitializeComponent();

            // Initialize the UDP clients

            udpVideoSender = new UdpClient();
            udpVideoReceiver = new UdpClient(videoStreamingPort);
            udpAudioSender = new UdpClient();
            udpAudioReceiver = new UdpClient(audioStreamingPort);

            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            Task.Run(() => ReceiveVideo());
            Task.Run(() => ReceiveAudio());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            foreach (FilterInfo device in videoDevices) selectVideoSourceComboBox.Items.Add(device.Name);
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (pictureBox1.IsDisposed) return;
            try
            {
                pictureBox1.Invoke((Action)(() =>
                {
                    Image? oldImage = pictureBox1.BackgroundImage;
                    Image image = (Image)eventArgs.Frame.Clone();
                    image.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    if (image.Width > 0 && image.Height > 0)
                    {
                        pictureBox1.BackgroundImage = image;
                    }

                    if (oldImage != null && oldImage != pictureBox1.BackgroundImage)
                    {
                        oldImage.Dispose();
                    }
                }));
            }
            catch { }

            // Create a new ResizeBilinear filter with the desired output size
            int newWidth = eventArgs.Frame.Width * 2 / 3;  // change to desired width
            int newHeight = eventArgs.Frame.Height * 2 / 3; // change to desired height
            ResizeBilinear filter = new (newWidth, newHeight);

            // Apply the filter
            Bitmap newFrame = filter.Apply(eventArgs.Frame);

            // Encode the frame as a JPEG image
            using (MemoryStream ms = new())
            {
                //eventArgs.Frame.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                newFrame.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] frameBytes = ms.ToArray();

                // Send the frame over UDP
                try
                {
                    if (udpVideoSender != null)
                    {
                        // Split the frame into chunks
                        int chunkSize = 64000; // Maximum size of a UDP packet (you might need to adjust this value)
                        for (int i = 0; i < frameBytes.Length; i += chunkSize)
                        {
                            bool isLastChunk = i + chunkSize >= frameBytes.Length;

                            byte[] chunk;
                            if (isLastChunk)
                            {
                                chunk = new byte[frameBytes.Length - i + 4]; // Include space for the marker
                                Array.Copy(frameBytes, i, chunk, 0, frameBytes.Length - i);
                                Array.Copy(endOfFrameMarker, 0, chunk, frameBytes.Length - i, 4);
                            }
                            else
                            {
                                chunk = new byte[chunkSize];
                                Array.Copy(frameBytes, i, chunk, 0, chunkSize);
                            }

                            udpVideoSender.SendAsync(chunk, chunk.Length, remoteIPAddressTextBox.Text, videoStreamingPort);
                        }
                    }
                }
                catch { }
            }
        }

        private async Task ReceiveVideo()
        {
            MemoryStream memoryStream = new();

            while (true)
            {
                UdpReceiveResult result;
                try
                {
                    result = await udpVideoReceiver.ReceiveAsync();

                    if (videoForm == null || videoForm.IsDisposed)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            if (videoForm == null)
                            {
                                videoForm = new VideoForm();
                                videoForm = VideoForm.instance;
                                videoForm.Show();
                            }
                        });
                    }
                }
                catch
                {
                    await Task.Delay(1000);
                    continue;
                }

                byte[] data = result.Buffer;

                // Check if the last four bytes are the end-of-frame marker
                bool endOfFrame = data[data.Length - 4] == 0xFF && data[data.Length - 3] == 0xFF
                    && data[data.Length - 2] == 0xFF && data[data.Length - 1] == 0xFF;

                if (!endOfFrame)
                {
                    memoryStream.Write(data, 0, data.Length);
                }
                else
                {
                    memoryStream.Write(data, 0, data.Length - 4); // Don't include the marker in the image data

                    memoryStream.Position = 0;
                    if (memoryStream.Length > 0)
                    {
                        try
                        {
                            using (Image receivedImage = Image.FromStream(memoryStream))
                            {
                                videoForm?.ProcessAndDisplayImage(receivedImage);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine("Invalid image data. Exception: " + ex.Message);
                        }
                    }

                    memoryStream.SetLength(0); // Clear the MemoryStream for the next frame
                }
            }
        }

        private async Task ReceiveAudio()
        {
            while (true)
            {
                UdpReceiveResult result;
                try
                {
                    result = await udpAudioReceiver.ReceiveAsync();
                }
                catch
                {
                    await Task.Delay(1000);
                    continue;
                }

                byte[] buffer = result.Buffer;
                if (buffer.Length > 0)
                {
                    // Decompress the audio data
                    short[] decompressedData = new short[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        decompressedData[i] = ALawDecoder.ALawToLinearSample(buffer[i]);
                    }
                    byte[] outputBuffer = new byte[decompressedData.Length * 2];
                    Buffer.BlockCopy(decompressedData, 0, outputBuffer, 0, outputBuffer.Length);

                    if (bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes >= outputBuffer.Length)
                    {
                        bufferedWaveProvider.AddSamples(outputBuffer, 0, outputBuffer.Length);
                    }
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (videoSource != null && videoSource.IsRunning)
            {
                Invoke((MethodInvoker)delegate
                {
                    videoSource.NewFrame -= videoSource_NewFrame;
                    videoSource.SignalToStop();
                    //videoSource.WaitForStop();
                    videoSource = null!;
                });
            }

            waveSource?.StopRecording();
            waveSource?.Dispose();

            waveOut?.Dispose();
            videoForm?.Close();
            videoForm?.Dispose();
        }

        private void selectVideoSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectVideoSourceComboBox.SelectedIndex == -1)
                return;

            if (remoteIPAddressTextBox.Text == "")
            {
                MessageBox.Show("Warning!", "Please provide an IP Address.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                selectVideoSourceComboBox.SelectedIndex = -1;
                return;
            }

            if (videoSource != null)
                videoSource.SignalToStop();
            videoSource = new VideoCaptureDevice(videoDevices[selectVideoSourceComboBox.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            videoSource.Start();

            waveSource = new();
            waveSource.DeviceNumber = selectVideoSourceComboBox.SelectedIndex;
            waveSource.WaveFormat = new WaveFormat(44100, 1);
            waveSource.DataAvailable += WaveSource_DataAvailable;
            waveSource.StartRecording();
        }

        private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (udpAudioSender != null)
            {
                try
                {
                    // Compress the audio data
                    byte[] compressedData = new byte[e.BytesRecorded / 2];
                    for (int i = 0; i < compressedData.Length; i++)
                    {
                        short sample = BitConverter.ToInt16(e.Buffer, i * 2);
                        compressedData[i] = ALawEncoder.LinearToALawSample(sample);
                    }

                    // Send the compressed data to the remote endpoint
                    udpAudioSender.SendAsync(compressedData, compressedData.Length, remoteIPAddressTextBox.Text, audioStreamingPort);
                }
                catch { }
            }
        }
    }
}