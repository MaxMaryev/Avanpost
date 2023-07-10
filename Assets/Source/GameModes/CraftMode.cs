public class CraftMode : IGameMode
{
    private AllCanvases _allCanvases;
    public CraftMode(AllCanvases allCanvases)
    {
        _allCanvases = allCanvases;
    }

    public void SetActive(bool state)
    {
        _allCanvases.gameObject.SetActive(!state);
    }
}
