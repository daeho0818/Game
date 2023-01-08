using System.Collections;
using System.Collections.Generic;
using GoogleSheetsToUnity;
using System;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    private const string sheetId = "18hK1P7opofnWgJ2b-qN56rskCKwLVknXNBc2SNzJxaM";
    private const string workSheetName = "Dialogue";

    public Action<List<Dialogue>> onDialogueLoaded;

    void Start() {
        onDialogueLoaded = (dialogues) => {
            foreach (var dialogue in dialogues) {
                var str = $"({dialogue.dialogueId}, {dialogue.comment})\n";
                foreach (var dialogueInfo in dialogue.dialogueInfos) {
                    str += $"{dialogueInfo.name}: {dialogueInfo.dialogue}\n";
                }
                
                Debug.Log(str);
            }
        };
        
        ReadSpreadSheet();
    }
    
    void ReadSpreadSheet() {
        SpreadsheetManager.Read(new GSTU_Search(sheetId, workSheetName), HandleSpreadSheetRead);
    }
    
    void HandleSpreadSheetRead(GstuSpreadSheet sheet) {
        var dialogues = new List<Dialogue>();

        var lastReadDialogue = null as Dialogue;
        foreach (var row in sheet.rows.primaryDictionary) {
            var values = row.Value;
            if (values.Find(v => v.value.Equals(Constants.SpreadSheetKey.DialogueId)) != null)
                continue;

            var dialogueID = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.DialogueId))?.value;
            var comment = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.Comment))?.value;
            var npcName = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.Name))?.value;
            var dialogueIndex = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.Index))?.value.ToInt() ?? -1;
            var dialogueText = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.Dialogue))?.value;
            
            var dialogueInfo = new DialogueInfo();
            dialogueInfo.name = npcName;
            dialogueInfo.index = dialogueIndex;
            dialogueInfo.dialogue = dialogueText;
            
            if (!string.IsNullOrEmpty(dialogueID)) {
                var dialogue = new Dialogue();
                lastReadDialogue = dialogue;
                dialogue.dialogueId = dialogueID;
                dialogue.comment = comment;
                dialogue.dialogueInfos.Add(dialogueInfo);
                
                dialogues.Add(dialogue);
            }
            else if (lastReadDialogue != null) {
                lastReadDialogue.dialogueInfos.Add(dialogueInfo);
            }
        }
        
        onDialogueLoaded?.Invoke(dialogues);
    }
}
