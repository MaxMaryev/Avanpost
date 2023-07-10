public interface IBreakable
{
    public bool IsBroken { get; }
    public bool IsRepaired { get; }

    public void Break(int damage);
    public void Repair();

    public void SetKinematic(bool state);
}
