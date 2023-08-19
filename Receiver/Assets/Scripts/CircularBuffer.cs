public class CircularBuffer
{
    private float[] buffer;
    private int start;
    private int end;

    public CircularBuffer(int capacity)
    {
        buffer = new float[capacity];
    }

    public void Write(float[] data)
    {
        foreach (float value in data)
        {
            buffer[end] = value;
            end = (end + 1) % buffer.Length;
            if (end == start)
            {
                start = (start + 1) % buffer.Length; // Overwrite oldest data when the buffer is full
            }
        }
    }

    public float Read()
    {
        if (start == end)
        {
            return 0; // Buffer is empty
        }

        float value = buffer[start];
        start = (start + 1) % buffer.Length;
        return value;
    }

    public bool IsEmpty
    {
        get { return start == end; }
    }
}
