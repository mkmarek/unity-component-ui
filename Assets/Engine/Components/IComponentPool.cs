namespace Assets.Engine.Components
{
    public interface IComponentPool
    {
        BaseUIComponent GetComponentByName(string name);
    }
}
