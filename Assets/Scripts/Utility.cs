using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    public static int ToInt(this object obj, int @default = -1) {
        int ret = @default;
        int.TryParse(obj.ToString(), out ret);
        return ret;
    }
}
