using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3ToWave
{
    class Program
    {
        static void Main(string[] args)
        {
            Mp3ToWav("Clk_1Sec1.mp3", "Clk_1Sec1.wav");
        }

        public static void Mp3ToWav(string mp3File, string outputFile)
        {
            using (Mp3FileReader reader = new Mp3FileReader(mp3File))
            {
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(outputFile, pcmStream);
                }

                using (var reader2 = new MediaFoundationReader(outputFile))
                {
                    string newMp3 = string.Format("New{0}", mp3File);
                    MediaFoundationEncoder.EncodeToMp3(reader2, newMp3, 192000);
                }
            }
        }
    }
}
