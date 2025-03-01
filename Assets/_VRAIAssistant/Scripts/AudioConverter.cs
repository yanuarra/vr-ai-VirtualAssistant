using UnityEngine;
using System;
using System.IO;

public class AudioConverter
{
    public static string ConvertAudioClipToWavBinary(AudioClip clip)
    {
        // Convert AudioClip to WAV byte array
        byte[] wavData = ConvertAudioClipToWav(clip);

        // Convert byte array to binary string
        return Convert.ToBase64String(wavData);
    }

    public static byte[] ConvertAudioClipToWav(AudioClip clip)
    {
        using (var memoryStream = new MemoryStream())
        {
            // Header chunk
            WriteString(memoryStream, "RIFF");
            WriteInt32(memoryStream, 0); // Final size will be written later
            WriteString(memoryStream, "WAVE");

            // Format chunk
            WriteString(memoryStream, "fmt ");
            WriteInt32(memoryStream, 16); // Chunk size
            WriteInt16(memoryStream, 1); // Audio format (1 = PCM)
            WriteInt16(memoryStream, (short)clip.channels);
            WriteInt32(memoryStream, clip.frequency);
            WriteInt32(memoryStream, clip.frequency * clip.channels * 2); // Byte rate
            WriteInt16(memoryStream, (short)(clip.channels * 2)); // Block align
            WriteInt16(memoryStream, 16); // Bits per sample

            // Data chunk
            WriteString(memoryStream, "data");
            WriteInt32(memoryStream, clip.samples * clip.channels * 2);

            // Convert audio data to WAV format
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            // Convert float samples to 16-bit PCM
            Int16[] intData = new Int16[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * 32767);
            }

            byte[] bytesData = new byte[intData.Length * 2];
            Buffer.BlockCopy(intData, 0, bytesData, 0, bytesData.Length);
            memoryStream.Write(bytesData, 0, bytesData.Length);

            // Write final size back to header
            var size = (int)memoryStream.Length - 8;
            memoryStream.Seek(4, SeekOrigin.Begin);
            WriteInt32(memoryStream, size);

            return memoryStream.ToArray();
        }
    }

    private static void WriteString(MemoryStream stream, string value)
    {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    private static void WriteInt32(MemoryStream stream, int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, 4);
    }

    private static void WriteInt16(MemoryStream stream, short value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        stream.Write(bytes, 0, 2);
    }
}