﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : Interactable
{
    [HideInInspector] public static bool isPossibleToTalk = true;
    [SerializeField] private string dialogueName;
    [SerializeField] private string talkerName;
    [SerializeField] private Animator actualTalkerAnimator;

    public string TalkerName
    {
        get { return talkerName; }
        set { talkerName = value; }
    }

    public string DialogueName
    {
        get { return dialogueName; }
        set { dialogueName = value; }
    }

    public Animator ActualTalkerAnimator
    {
        get
        {
            if (actualTalkerAnimator)
                return actualTalkerAnimator;
            else
                return GetComponent<Animator>();
        }

        set { actualTalkerAnimator = value; }
    }

    // Use this for initialization
    void Start ()
    {

    }

    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        GameObject Canvas = GameObject.Find("CanvasPlayerUI");
        Canvas.GetComponent<DialogueManager>().InitDialogue(this);
    }
}
