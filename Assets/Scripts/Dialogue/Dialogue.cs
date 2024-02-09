using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dialogue {
    public string dialogueId;
    public string comment;
    public List<DialogueInfo> dialogueInfos = new List<DialogueInfo>();

}

[Serializable]
public class DialogueInfo {
    public string name;
    public int index;
    public string dialogue;
}