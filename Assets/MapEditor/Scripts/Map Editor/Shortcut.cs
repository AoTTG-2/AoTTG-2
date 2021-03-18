using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Serializes the keys of the shortcut and checks if they are pressed or held
[Serializable]
public class Shortcut
{
    private enum ModifierKey
    {
        None,
        LeftShift,
        LeftAlt
    }

    //Determines if the control or command key needs to be held
    [SerializeField] private bool requireCommandKey;

    //An optional modifier key 
    [FormerlySerializedAs("modifyerKey")] [SerializeField] private ModifierKey modifierKey;

    //The last key in the sequence which executes the shortcut
    [SerializeField] private KeyCode executeKey;

    //Returns true if all of shortcut keys except for the execute key are held
    private bool ModifiersHeld()
    {
        //Return false if the control or command key is required but not held
        if (requireCommandKey && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftCommand))
            return false;

        //If modifier keys are held when they aren't supposed to be, return false
        if (modifierKey == ModifierKey.None && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftShift)))
            return false;

        //If the alt key isn't pressed when it should be or the shift key is also pressed, return false
        if (modifierKey == ModifierKey.LeftAlt && (!Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftShift)))
            return false;

        //If the shift key isn't pressed when it should be or the alt key is also pressed, return false
        if (modifierKey == ModifierKey.LeftShift && (!Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftAlt)))
            return false;

        //If all of the correct keys are held, return true
        return true;
    }

    //Returns true the frame the shortcut is pressed
    public bool Pressed()
    {
        return (ModifiersHeld() && Input.GetKeyDown(executeKey));
    }

    //Returns true while the shortcut is held
    public bool Held()
    {
        return (ModifiersHeld() && Input.GetKey(executeKey));
    }
}

//Contains multiple shortcuts for the same function
[Serializable]
public class MultiShortcut
{
    [SerializeField] private List<Shortcut> keyCombinations;

    //Returns true the frame the shortcut is pressed
    public bool Pressed()
    {
        foreach (Shortcut shortcut in keyCombinations)
            if (shortcut.Pressed())
                return true;

        return false;
    }

    //Returns true while the shortcut is held
    public bool Held()
    {
        foreach (Shortcut shortcut in keyCombinations)
            if (shortcut.Held())
                return true;

        return false;
    }
}