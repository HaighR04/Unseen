using UnityEngine;
using System;

public enum BodyPartType {Eyes, Legs, Arms}

public class PlayerAbilities : MonoBehaviour
{
    public bool HasEyes {get; private set;}
    public bool HasLegs {get; private set;}
    public bool HasArms {get; private set;}
    
    public event Action<BodyPartType> OnUnlocked;

    public void Unlock(BodyPartType part) {
        switch (part) {
            case BodyPartType.Eyes: HasEyes = true; break;
            case BodyPartType.Legs: HasLegs = true; break;
            case BodyPartType.Arms: HasArms = true; break;
            // Add more bodyparts later if implemented
        }
        OnUnlocked?.Invoke(part);
    }
}
