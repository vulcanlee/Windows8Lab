using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaRecorder
{
    public enum AudioEncodingFormat
    {
        Mp3,
        Mp4,
        Avi,
        Wma
    };

    public static class AudioEncodingFormatExtensions
    {
        public static string ToFileExtension(this AudioEncodingFormat encodingFormat)
        {
            switch (encodingFormat)
            {
                case AudioEncodingFormat.Mp3:
                    return ".mp3";
                case AudioEncodingFormat.Mp4:
                    return ".mp4";
                case AudioEncodingFormat.Avi:
                    return ".avi";
                case AudioEncodingFormat.Wma:
                    return ".wma";              
                default:
                    throw new ArgumentOutOfRangeException("encodingFormat");
            }
        }
    }
}
