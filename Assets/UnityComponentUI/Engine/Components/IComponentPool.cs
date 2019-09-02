namespace UnityComponentUI.Engine.Components
{
    public interface IComponentPool
    {
        IBaseUIComponent GetComponentByName(string name);
    }
}
