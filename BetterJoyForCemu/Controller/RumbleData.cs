using System;
using System.Collections.Generic;

namespace BetterJoyForCemu.Controller {
    /// <summary>
    /// Manages rumble data and encoding for Joy-Con controllers
    /// </summary>
    public class RumbleData {
        private readonly Queue<float[]> queue = new Queue<float[]>();

        public int QueueCount => queue.Count;

        public RumbleData(float lowFreq, float highFreq, float amplitude = 0f) {
            queue.Enqueue(new[] { lowFreq, highFreq, amplitude });
        }

        public void SetValues(float lowFreq, float highFreq, float amplitude) {
            // Keep a queue of 15 items, discard oldest if full
            if (queue.Count > 15) {
                queue.Dequeue();
            }
            queue.Enqueue(new[] { lowFreq, highFreq, amplitude });
        }

        private static float Clamp(float value, float min, float max) {
            return Math.Max(min, Math.Min(max, value));
        }

        private static byte EncodeAmplitude(float amp) {
            if (amp == 0)
                return 0;
            
            if (amp < 0.117)
                return (byte)(((Math.Log(amp * 1000, 2) * 32) - 0x60) / (5 - Math.Pow(amp, 2)) - 1);
            
            if (amp < 0.23)
                return (byte)(((Math.Log(amp * 1000, 2) * 32) - 0x60) - 0x5c);
            
            return (byte)((((Math.Log(amp * 1000, 2) * 32) - 0x60) * 2) - 0xf6);
        }

        public byte[] GetData() {
            byte[] rumble_data = new byte[8];
            
            if (queue.Count == 0) {
                // Default neutral rumble
                Array.Copy(new byte[] { 0x0, 0x1, 0x40, 0x40 }, rumble_data, 4);
                Array.Copy(rumble_data, 0, rumble_data, 4, 4);
                return rumble_data;
            }

            float[] queued_data = queue.Dequeue();

            if (queued_data[2] == 0.0f) {
                rumble_data[0] = 0x0;
                rumble_data[1] = 0x1;
                rumble_data[2] = 0x40;
                rumble_data[3] = 0x40;
            } else {
                queued_data[0] = Clamp(queued_data[0], 40.875885f, 626.286133f);
                queued_data[1] = Clamp(queued_data[1], 81.75177f, 1252.572266f);
                queued_data[2] = Clamp(queued_data[2], 0.0f, 1.0f);

                ushort hf = (ushort)((Math.Round(32f * Math.Log(queued_data[1] * 0.1f, 2)) - 0x60) * 4);
                byte lf = (byte)(Math.Round(32f * Math.Log(queued_data[0] * 0.1f, 2)) - 0x40);
                byte hf_amp = EncodeAmplitude(queued_data[2]);

                ushort lf_amp = (ushort)(Math.Round((double)hf_amp) * 0.5);
                byte parity = (byte)(lf_amp % 2);
                
                if (parity > 0) {
                    --lf_amp;
                }

                lf_amp = (ushort)(lf_amp >> 1);
                lf_amp += 0x40;
                
                if (parity > 0) 
                    lf_amp |= 0x8000;

                // Make even to prevent weird hum
                hf_amp = (byte)(hf_amp - (hf_amp % 2));
                
                rumble_data[0] = (byte)(hf & 0xff);
                rumble_data[1] = (byte)(((hf >> 8) & 0xff) + hf_amp);
                rumble_data[2] = (byte)(((lf_amp >> 8) & 0xff) + lf);
                rumble_data[3] = (byte)(lf_amp & 0xff);
            }

            // Copy to second half
            Array.Copy(rumble_data, 0, rumble_data, 4, 4);

            return rumble_data;
        }
    }
}
