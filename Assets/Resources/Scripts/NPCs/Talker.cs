﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    [HideInInspector] public static bool isPossibleToTalk = true;
    [SerializeField] private string dialogueName;
    [SerializeField] private string talkerName;

    public string TalkerName
    {
        get { return talkerName; }
    }

    public string DialogueName
    {
        get { return dialogueName; }
    }

    // Use this for initialization
    void Start ()
    {
        GameObject Canvas = GameObject.Find("CanvasPlayerUI");
        Canvas.GetComponent<DialogueManager>().InitDialogue(this);
    }

}