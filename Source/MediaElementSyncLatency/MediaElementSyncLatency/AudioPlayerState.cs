﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 音樂同步測試
{
    public enum AudioPlayerState
    {
        /// <summary>
        /// The player is stopped (default).
        /// </summary>
        Stopped,

        /// <summary>
        /// The player is playing a sound.
        /// </summary>
        Playing,

        /// <summary>
        /// The player is paused.
        /// </summary>
        Paused,
    }
}
