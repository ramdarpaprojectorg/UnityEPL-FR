﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyText : MonoBehaviour
{
    public UnityEngine.UI.Text textElement;

    public float startZ = 3;
    public float hoverTime = 1.6f;

    bool is_encoding = false;

    void OnEnable()
    {
        EditableExperiment.OnStateChange += OnStateChange;
        TextDisplayer.OnText += OnText;
    }

    void OnDisable()
    {
        EditableExperiment.OnStateChange -= OnStateChange;
        TextDisplayer.OnText -= OnText;
    }

    public void OnStateChange(string name, bool on, Dictionary<string, object> extraData)
    {
        if (name.Equals("ENCODING"))
            is_encoding = on;
    }

    public void OnText(string text)
    {
        if (is_encoding)
            StartCoroutine(DoFly());
    }

    private IEnumerator DoFly()
    {
        //Debug.Log ("DoFly");
        float startTime = Time.time;
        Vector3 startPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, startZ);
        gameObject.transform.localPosition = startPosition;
        Vector3 hoverPosition = gameObject.transform.position;
        while (Time.time < startTime + hoverTime)
        {
            gameObject.transform.position = new Vector3(hoverPosition.x, gameObject.transform.position.y, hoverPosition.z);
            yield return null;
        }
        gameObject.transform.localPosition = startPosition;
    }
}