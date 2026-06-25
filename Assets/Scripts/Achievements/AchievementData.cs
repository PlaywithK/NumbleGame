using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public Sprite icon;
    public bool unlocked = false;
}