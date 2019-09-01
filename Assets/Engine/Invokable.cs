using System;

namespace Assets.Engine
{
    public class Invokable
    {
        public bool Invoked { get; private set; }
        public Action Action { get; private set; }

        public Invokable()
        {
            Invoked = false;
            Action = () => { Invoked = true; };
        }

        public void Reset()
        {
            Invoked = false;
        }
    }
}
