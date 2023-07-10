using UnityEngine;

public static class Layers
{
    public static int Ground { get => LayerMask.NameToLayer(nameof(Ground)); }
    public static int Enemy { get => LayerMask.GetMask(nameof(Enemy)); }
    public static int Camera { get => LayerMask.GetMask(nameof(Camera)); }
    public static int Light { get => LayerMask.GetMask(nameof(Light)); }
    public static int Building { get => LayerMask.GetMask(nameof(Building)); }
    public static int Player { get => LayerMask.GetMask(nameof(Player)); }
}
