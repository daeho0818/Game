using System.Collections;
using System.Collections.Generic;
using GoogleSheetsToUnity;
using System;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    private const string SHEET_ID = "18hK1P7opofnWgJ2b-qN56rskCKwLVknXNBc2SNzJxaM";
    private const string WORK_SHEET_NAME = "Dialogue";

    public Action<List<Dialogue>> onDialogueLoaded;

    private void Start() {
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
    
    private void ReadSpreadSheet() {
        SpreadsheetManager.Read(new GSTU_Search(SHEET_ID, WORK_SHEET_NAME), HandleSpreadSheetRead);
    }
    
    private void HandleSpreadSheetRead(GstuSpreadSheet sheet) {
        var dialogues = new List<Dialogue>();

        Dialogue lastReadDialogue = null;
        foreach (var row in sheet.rows.primaryDictionary) {
            var values = row.Value;
            if (values.Find(v => v.value.Equals(Constants.SpreadSheetKey.DIALOGUE_ID)) != null)
                continue;

            var dialogueID = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.DIALOGUE_ID))?.value;
            var comment = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.COMMENT))?.value;
            var npcName = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.NAME))?.value;
            var dialogueIndex = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.INDEX))?.value.ToInt() ?? -1;
            var dialogueText = values.Find(v => v.columnId.Equals(Constants.SpreadSheetKey.DIALOGUE))?.value;
            
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
            } else if (lastReadDialogue != null) {
                lastReadDialogue.dialogueInfos.Add(dialogueInfo);
            }
        }
        
        onDialogueLoaded?.Invoke(dialogues);
    }
}
