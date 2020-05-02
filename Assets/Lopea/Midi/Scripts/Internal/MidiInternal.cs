//Filename:    MidiInternal.cs 
//Author:      Javier Sandoval (Lopea)
//GitHub:      https://github.com/lopea
//Description: Contains functions to link RtMidi to current project.

using System;
using System.Runtime.InteropServices;

namespace Lopea.Midi.Internal
{
    public enum RtMidiApi
    {
        RTMIDI_API_UNSPECIFIED,
        RTMIDI_API_MACOSX_CORE,
        RTMIDI_API_LINUX_ALSA,
        RTMIDI_API_UNIX_JACK,
        RTMIDI_API_WINDOWS_MM,
        RTMIDI_API_RTMIDI_DUMMY,
        RTMIDI_API_NUM
    }

    public delegate void RtMidiCCallback(double timeStamp, IntPtr message, int messageSize, IntPtr userData);

    public class MidiInternal
    {

        // get platform specific libraries
#if (UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_LINUX_API)
        const string dllname = "RtLinux";
#elif (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_STANDALONE_WIN_API)
    const string dllname = "RtWin";
#endif

        /* RtMidi API functions */
        
        //gets api that was used for the RtMidi Device given.
        [DllImport(dllname)]
        public static extern RtMidiApi rtmidi_in_get_current_api(IntPtr device);
        
        
        /* RtMidi Input device functions */


        //Creates an RtMidi input device with the api, name and size limit given.
        //Returns a pointer refencing the new input device handler
        [DllImport(dllname)]
        public static extern IntPtr rtmidi_in_create(RtMidiApi api, string clientName, uint queueSizeLimit);

        //creates an input device with the default settings 
        [DllImport(dllname)]
        public static extern IntPtr rtmidi_in_create_default();

        //frees the RtMidi input device pointer with pointer given
        [DllImport(dllname)]
        public static extern void rtmidi_in_free(IntPtr device);

        //gets the amount of midi ports available
        //returns the number of ports available 
        [DllImport(dllname)]
        public static extern uint rtmidi_get_port_count(IntPtr device);

        //opens a port and connects it to the input port given
        [DllImport(dllname)]
        public static extern void rtmidi_open_port(IntPtr device, uint portNumber, string portName);

        //closes port that is connected to input device given
        [DllImport(dllname)]
        public static extern void rtmidi_close_port(IntPtr device);

        //get name of port based on the port number given
        //returns name of port
        [DllImport(dllname)]
        public static extern string rtmidi_get_port_name(IntPtr device, uint portNumber);

        //set callback function to device handler 
        //NOTE: this function should be executed BEFORE opening a port to avoid any overflows
        [DllImport(dllname)]
        public static extern void rtmidi_in_set_callback(IntPtr device,
                                                         [MarshalAs(UnmanagedType.FunctionPtr)]
                                                         RtMidiCCallback callback,
                                                         IntPtr userData);
        
        //removes the callback from input device 
        [DllImport(dllname)]
        public static extern void rtmidi_in_cancel_callback(IntPtr device);
        
        //set midi messages to ignore when receiving messages from device given
        [DllImport(dllname)]
        public static extern void rtmidi_in_ignore_types(IntPtr device, [MarshalAs(UnmanagedType.Bool)] bool midiSysex, 
                                                                        [MarshalAs(UnmanagedType.Bool)] bool midiTime, 
                                                                        [MarshalAs(UnmanagedType.Bool)] bool midiSense);

        //get current message from device given
        //returns delta time in seconds
        [DllImport(dllname)]
        public static extern double rtmidi_in_get_message(IntPtr device, [In, Out] IntPtr message, [Out] IntPtr size);

        /* RtMidi output device functions */

        //creates an output device with the default settings 
        [DllImport(dllname)]
        public static extern IntPtr rtmidi_out_create_default();

        //frees the RtMidi output device pointer with pointer given
        [DllImport(dllname)]
        public static extern void rtmidi_out_free(IntPtr device);

        //gets api that was used for the RtMidi Output Device given.
        [DllImport(dllname)]
        public static extern RtMidiApi rtmidi_out_get_current_api(IntPtr device);
        
        //send message to port that is connected to device given.
        //returns 0 on success, -1 on error
        [DllImport(dllname)]
        public static extern int rtmidi_out_send_message(IntPtr device, byte[] message, int length);
  
    }
}