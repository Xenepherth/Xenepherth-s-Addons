using Mirror;
using UnityEngine;

public class UCE_UI_SettingsVariables : MonoBehaviour
{
    public KeyCode[] keybindings = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.LeftControl,
        KeyCode.Space, KeyCode.LeftAlt, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.R,
        KeyCode.T, KeyCode.Y, KeyCode.M, KeyCode.C, KeyCode.B, KeyCode.F };

    [HideInInspector] public bool isShowOverhead = true;
    [HideInInspector] public bool isShowChat = true;

    [HideInInspector]
    public bool[] keybindUpdate = new bool[] { false, false, false, false, false, false, false,
     false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
}