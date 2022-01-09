using System.Collections;

namespace MediaPanelController.Communication
{
    public delegate void SerialMessageContainer_MessageIncoming();

    internal class SerialMessageContainer : ArrayList
    {
        public event SerialMessageContainer_MessageIncoming MessageIncoming;
        
        protected virtual void OnChanged()
        {
            MessageIncoming();
        }

        public new void Insert(int index, object value)
        {
            base.Insert(index, value);
            OnChanged();
        }

        public new void Add(object value)
        {
            Insert(0, value);
        }

        public void Push(object value)
        {
            Add(value);
        }

        public object Pop()
        {
            object lastObject = null;

            if (base.Count > 0)
            {
                lastObject = base[base.Count - 1];
                base.Remove(base.Count - 1);
            }

            return lastObject;
        }

        public object Shift()
        {
            object firstObject = null;

            if (base.Count > 0)
            {
                firstObject = base[0];
                base.Remove(0);
            }

            return firstObject;
        }

        public void Unshift(object value)
        {
            Insert(base.Count, value);
        }
    }
}
