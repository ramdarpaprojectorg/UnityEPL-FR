using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ltpDelayRepFRExperiment : DelayRepFRExperiment {
    public ltpDelayRepFRExperiment(InterfaceManager _manager) : base(_manager) {}

    public override StateMachine GetStateMachine() {
        StateMachine stateMachine = new StateMachine(currentSession);

        stateMachine["Run"] = new ExperimentTimeline(
            new List<Action<StateMachine>> {
                QuitPrompt,
                IntroductionPrompt,
                IntroductionVideo,
                RepeatVideo,
                MicrophoneTest, // runs MicrophoneTest states

                Practice, // runs Practice states
                ConfirmStart,
                MainLoop, // runs MainLoop states

                FinishExperiment});

        // though it is largely the same as the main loop,
        // practice is a conceptually distinct state machine
        // that just happens to overlap with MainLoop
        stateMachine["Practice"] = new LoopTimeline(
            new List<Action<StateMachine>> {
                StartTrial,
                NextPracticeListPrompt,
                PreCountdownRest,
                CountdownVideo,
                EncodingDelay,
                Encoding,
                Rest,
                RecallPrompt,
                Recall,
                EndPracticeTrial});

        stateMachine["MainLoop"] = new LoopTimeline(
            new List<Action<StateMachine>> {
                StartTrial,
                NextListPrompt,
                PreCountdownRest,
                CountdownVideo,
                EncodingDelay,
                Encoding,
                Rest,
                RecallPrompt,
                Recall,
                ParticipantBreak,
                EndTrial});

        stateMachine["MicrophoneTest"] = new LoopTimeline(
            new List<Action<StateMachine>> {
                MicTestPrompt,
                RecordTest,
                RepeatMicTest});

        stateMachine.PushTimeline("Run");
        return stateMachine;
    }

    protected void ParticipantBreak(StateMachine state) {
        // check if this list exists in the configuration rest list
        if (Array.IndexOf(manager.GetSetting("restLists"), state.currentSession.GetListIndex()) != -1) {
            Do(new EventBase<StateMachine>(WaitForResearcher, state));
        } else {
            state.IncrementState();
            Run();
        }
    }

    //////////
    // Wait Functions
    //////////

    protected void WaitForResearcher(StateMachine state) {
        WaitForKey("participant break",
                    "It's time for a short break, please " +
                    "wait for the researcher to come check on you " +
                    "before continuing the experiment. \n\n" +
                    "Researcher: press space to resume the experiment.",
                    "space");
    }
}
