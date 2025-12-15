using UnityEngine;

public enum RingQuality
{
    Perfect,
    Broken,
}

[CreateAssetMenu(fileName = "Ring", menuName = "Game/Ring Object")]

public class RingObject : ScriptableObject
{
    public Sprite ringSprite;
    public RingQuality quality;
}
