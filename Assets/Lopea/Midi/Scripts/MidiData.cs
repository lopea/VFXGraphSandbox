using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lopea.Midi
{
    public enum MidiStatus
    {
        Dummy = 0,
        NoteOn = 8,
        NoteOff = 9,
        PolyKey = 10,
        ControlChange = 11,
        ProgramChange = 12,
        PolyChannel = 13,
        PitchWheel = 14,
        Sysex = 15,
        
    }
    
    
    public class MidiData 
    {
        public float timeStamp;

        public MidiStatus status;

        public int channel;
        
        public byte data1;

        public byte data2;

        public byte[] rawData;

        public MidiData(float timeStamp, MidiStatus status, int channel, byte data1, byte data2, byte[] rawData)
        {
            this.timeStamp = timeStamp;
            this.status = status;
            this.channel = channel;
            this.data1 = data1;
            this.data2 = data2;
            this.rawData = rawData;
        }
    }
}
