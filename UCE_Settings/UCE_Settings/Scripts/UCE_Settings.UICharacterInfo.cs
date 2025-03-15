using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if _iMMOATTRIBUTES

// Sets our new hotkeys for character info.
public partial class UCE_UI_CharacterInfoAttributes : MonoBehaviour
{
    private UCE_UI_SettingsVariables settingsVariables;

    // Grabs our settings variables.
    private void Start()
    {
        settingsVariables = FindObjectOfType<UCE_UI_SettingsVariables>().GetComponent<UCE_UI_SettingsVariables>();
    }

    // Set our hotkey based on the players selection.
    private void FixedUpdate()
    {
        if (settingsVariables != null)
            if (settingsVariables.keybindUpdate[21])
            {
                hotKey = settingsVariables.keybindings[21];
                settingsVariables.keybindUpdate[21] = false;
            }
    }
}

#else

// Sets our new hotkeys for character info.
public partial class UICharacterInfo : MonoBehaviour
{
    private UCE_UI_SettingsVariables settingsVariables;

    // Grabs our settings variables.
    private void Start()
    {
        settingsVariables = FindObjectOfType<UCE_UI_SettingsVariables>().GetComponent<UCE_UI_SettingsVariables>();
    }

    // Set our hotkey based on the players selection.
    private void FixedUpdate()
    {
        if (settingsVariables != null)
            if (settingsVariables.keybindUpdate[21])
            {
                hotKey = settingsVariables.keybindings[21];
                settingsVariables.keybindUpdate[21] = false;
            }
    }
}

#endif