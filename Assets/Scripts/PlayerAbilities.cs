using UnityEngine;
using System;

public enum BodyPartType {Eyes, Legs, Arms}

// Creates the settings for each body part that is passed into other functions
public class PlayerAbilities : MonoBehaviour
{
    // Getter and setter for each body part, set is private so only this class can alter it
    public bool HasEyes {get; private set;}
    public bool HasLegs {get; private set;}
    public bool HasArms {get; private set;}
    
    public event Action<BodyPartType> OnUnlocked;

    // Function for actually "unlocking" the body parts individually
    public void Unlock(BodyPartType part) {
        switch (part) {
            case BodyPartType.Eyes: HasEyes = true; break;
            case BodyPartType.Legs: HasLegs = true; break;
            case BodyPartType.Arms: HasArms = true; break;
        }
        OnUnlocked?.Invoke(part);
    }
}
