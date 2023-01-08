using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public static class Constants {
    public static class SpreadSheetKey {
        public readonly static string DialogueId = "Dialogue ID";
        public readonly static string Comment = "Comment";
        public readonly static string Name = "NPC Name";
        public readonly static string Index = "Index";
        public readonly static string Dialogue = "Dialogue Text";
    }

    public static int ToInt(this object obj, int _default = -1) {
        int ret = _default;
        int.TryParse(obj.ToString(), out ret);
        return ret;
    }
}
