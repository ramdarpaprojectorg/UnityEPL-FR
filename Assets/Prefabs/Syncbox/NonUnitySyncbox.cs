﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Threading;

public class NonUnitySyncbox : EventLoop 
{
    public InterfaceManager im;

    //Function from Corey's Syncbox plugin (called "ASimplePlugin")
	[DllImport ("ASimplePlugin")]
	private static extern IntPtr OpenUSB();
	[DllImport ("ASimplePlugin")]
	private static extern IntPtr CloseUSB();
	[DllImport ("ASimplePlugin")]
	private static extern IntPtr TurnLEDOn();
	[DllImport ("ASimplePlugin")]
	private static extern IntPtr TurnLEDOff();
	[DllImport ("ASimplePlugin")]
	private static extern float SyncPulse();

    private const int PULSE_START_DELAY = 1000; // ms
    private const int TIME_BETWEEN_PULSES_MIN = 800;
    private const int TIME_BETWEEN_PULSES_MAX = 1200;

    private volatile bool stopped = true;

    public int testField;

    public NonUnitySyncbox(InterfaceManager _im) {
        im = _im;
    }

    public void Init() {
        Debug.Log(Marshal.PtrToStringAuto(OpenUSB()));
        Debug.Log(testField);

        StopPulse();
        Start();
    }

    public bool IsRunning() {
        return !stopped;
    }

    public void StartPulse() {
        if (!IsRunning())
        {
            stopped = false;
            DoIn(new EventBase(Pulse), PULSE_START_DELAY);
        }
    }

	private void Pulse ()
    {
		if(!stopped)
        {
            // Send a pulse
            Debug.Log("Pew!");
            im.scriptedInput.ReportOutOfThreadScriptedEvent("syncPulse", new System.Collections.Generic.Dictionary<string, object>());
            SyncPulse();

            // Wait a random interval between min and max
            int timeBetweenPulses = (int)(TIME_BETWEEN_PULSES_MIN + (int)(InterfaceManager.rnd.NextDouble() * (TIME_BETWEEN_PULSES_MAX - TIME_BETWEEN_PULSES_MIN)));
            DoIn(new EventBase(Pulse), timeBetweenPulses);
		}
	}

    public void StopPulse() {
        stopped = true;
    }
}
