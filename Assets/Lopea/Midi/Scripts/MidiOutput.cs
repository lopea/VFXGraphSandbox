using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lopea.Midi.Internal;

namespace Lopea.Midi
{
    public class MidiOutput : MonoBehaviour
    {
        static IntPtr[] OutPorts;
        static bool _init;
        static GameObject _handler;

        public static void Initialize()
        {
            if(_init)
                return;
            //both midi in and midi out have the same ports
            uint portCount = MidiInput.GetPortCount();
            
            //check if there are no midi devices available
            if(portCount == 0)
            {
                //print warning
                Debug.LogError("MIDIOUT: No Midi Output Devices Found!");

                //leave
                return;
            }
            
            
            //setup output device handles
            OutPorts = new IntPtr[portCount];

            for(uint i = 0; i < portCount; i ++)
            {
                OutPorts[i] = MidiInternal.rtmidi_out_create_default();
                MidiInternal.rtmidi_open_port(OutPorts[i], i, "LopeaMidi: Out " + i);    
            }
            _handler = new GameObject("Midi Output");
            _handler.hideFlags = HideFlags.HideInHierarchy;
            _handler.AddComponent<MidiOutput>();
            _init = true;
        }

        public static void Shutdown()
        {
            //check if the device was already shutdown
            if(!_init)
                return;
            
            //free all handles...
            for(int i = 0; i < OutPorts.Length; i ++)
            {
                MidiInternal.rtmidi_out_free(OutPorts[i]);
                OutPorts[i] = IntPtr.Zero;
            }
            
            //erase everything
            OutPorts = null;
            _handler = null;
            //set init flag to false
            _init = false;
        }

        public static void SendRawData(uint port, byte[] data)
        {
            if (!_init)
            {
                //setup values
                Initialize();

                //if initialization did not happen, print error message
                if (!_init)
                {
                    //if something went wrong, print error
                    Debug.LogError("Data to MIDI not sent!\n An error occured during the Initialization process!");
                    return;
                }
            }
            //check if port is valid
            if(port < OutPorts.Length)
            {
                //send message
                MidiInternal.rtmidi_out_send_message(OutPorts[port], data, data.Length);
            }
            else
            {
                Debug.LogError("Device port #" + port + " is invalid for output!");
            }
        }

        
        public static void SendData(uint port, MidiData data)
        {
            //cannot do anything with a dummy 
            if(data.status == MidiStatus.Dummy)
                return;

            SendRawData(port, data.rawData);
        }

        public static void SendSimpleData(uint port, MidiStatus status, byte data1, byte data2, byte channel = 0)
        {

            byte[] data = { (byte)(((int)status << 4) | (channel & 0xF)), data1, data2};

            SendRawData(port, data);
        }


        void OnDisable()
        {
            Shutdown();
        }

    }

}
