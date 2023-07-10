using UnityEngine;

public interface ICraftableItem 
{
    public string Name { get; }
    public string Description { get; }
    public Sprite Icon { get; }

    public int JunkNeeded { get; }
    public Sprite Poster { get; }
}
