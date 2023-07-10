public class HealthBar : Bar
{
    private IBarDisplayable _health;

    public void Init(IBarDisplayable health)
    {
        _health = health;
        OnValueChanged(health.MaxValue, health.CurrentValue);
        _health.ValueChanged += OnValueChanged;
    }
}
