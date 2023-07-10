using UnityEngine;

public class OilLamp : Lamp
{
    private LightHandler _lightHandler;

    private void OnDestroy()
    {
        if (_lightHandler != null)
            _lightHandler.DeleteLight(Light);
    }

    public void Init(LightHandler lightHandler)
    {
        _lightHandler = lightHandler;
        _lightHandler.AddLight(Light);
    }
}
