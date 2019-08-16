﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the button which launches the experiment.
/// 
/// DoLaunchExperiment is responsible for calling EditableExperiment.ConfigureExperiment with the proper parameters.
/// </summary>
public class UpdatedLaunchExperiment : MonoBehaviour
{
    public InterfaceManager manager;
    public GameObject cantGoPrompt;
    public UnityEngine.UI.InputField participantNameInput;
    public UnityEngine.GameObject launchButton;
    public UnityEngine.GameObject greyedLaunchButton;
    public UnityEngine.GameObject loadingButton;

    void Awake() {
        GameObject mgr = GameObject.Find("InterfaceManager");
        manager = (InterfaceManager)mgr.GetComponent("InterfaceManager");
    }
    void Update()
    {
        launchButton.SetActive(isValidParticipant(participantNameInput.text));
        greyedLaunchButton.SetActive(!launchButton.activeSelf);

        if (isValidParticipant(participantNameInput.text))
        {
            int sessionNumber = UpdatedParticipantSelection.nextSessionNumber;
            launchButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Start session " + sessionNumber.ToString();
        }
    }

    // activated by UI launch button
    public void DoLaunchExperiment()
    {
       if (participantNameInput.text.Equals(""))
        {
            cantGoPrompt.GetComponent<UnityEngine.UI.Text>().text = "Please enter a participant";
            cantGoPrompt.SetActive(true);
            return;
        }
        if (!isValidParticipant(participantNameInput.text))
        {
            cantGoPrompt.GetComponent<UnityEngine.UI.Text>().text = "Please enter a valid participant name (ex. R1123E or LTP123)";
            cantGoPrompt.SetActive(true);
            return;
        }

        int sessionNumber = UpdatedParticipantSelection.nextSessionNumber;

        manager.ChangeSetting("participantCode", participantNameInput.text);
        manager.ChangeSetting("session", sessionNumber);

        launchButton.SetActive(false);
        loadingButton.SetActive(true);

        manager.Do(new EventBase(manager.LaunchExperiment));
    }

    private bool isValidParticipant(string name)
    {
        return manager.fileManager.isValidParticipant(name);
    }
}