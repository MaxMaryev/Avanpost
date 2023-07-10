public class ResourceBox 
{
    public Resource Resource { get; }
    public int Amount { get; }

    public ResourceBox(Resource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
}
