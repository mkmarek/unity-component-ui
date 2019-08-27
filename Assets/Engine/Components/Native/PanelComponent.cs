using UnityEngine;

namespace Assets.Engine.Components.Native
{
    public class PanelComponent : IBaseUIComponent
    {
        public void Render()
        {
            GUI.Box(new Rect(0, 0, 200, 200), "Some stuff here" );
        }
    }
}
