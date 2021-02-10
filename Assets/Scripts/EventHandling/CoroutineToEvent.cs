using System;
using System.Collections;

public class CoroutineToEvent {
    public static void StartCoroutine(IEnumerator coroutine, EventQueue queue) {
        queue.Do(new EventBase(() => {
                if(coroutine.MoveNext()) {
                    CoroutineToEvent.StartCoroutine(coroutine, queue);
                }
            }
        ));
    }

    // TODO:
    // StopCoroutine
    // StopAllCoroutines
}