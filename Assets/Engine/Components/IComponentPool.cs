namespace Assets.Engine.Components
{
    public interface IComponentPool
    {
        IBaseUIComponent GetComponentByName(string name);
    }
}
