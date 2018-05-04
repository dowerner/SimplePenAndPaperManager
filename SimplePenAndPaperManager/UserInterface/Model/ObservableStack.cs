using System.Collections.Generic;

namespace SimplePenAndPaperManager.UserInterface.Model
{
    public class ObservableStack<T> : Stack<T>
    {
        public delegate void StackChangedHandler(object sender, StackChangedEventArgs<T> e);
        public event StackChangedHandler StackChanged;

        public new T Pop()
        {
            T item = base.Pop();
            StackChanged?.Invoke(this, new StackChangedEventArgs<T>() { Item = item, EventType = StackChangedEvent.Pop });
            return item;
        }

        public new void Push(T item)
        {
            base.Push(item);
            StackChanged?.Invoke(this, new StackChangedEventArgs<T>() { Item = item, EventType = StackChangedEvent.Push });
        }
    }

    public struct StackChangedEventArgs<T>
    {
        public T Item { get; set; }
        public StackChangedEvent EventType { get; set; }
    }

    public enum StackChangedEvent
    {
        Push,
        Pop
    }
}
