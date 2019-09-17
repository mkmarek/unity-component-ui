using System.Collections.Generic;

namespace UnityComponentUI.Engine
{
    public class ComponentResources
    {
        private static readonly Dictionary<string, object> Resources = new Dictionary<string, object>();

        public static void Register<T>(string alias, T resource)
        {
            Resources.Add(alias, resource);
        }

        public static object Get(string alias)
        {
            if (!Resources.ContainsKey(alias))
            {
                return null;
            }

            return Resources[alias];
        }
    }
}
