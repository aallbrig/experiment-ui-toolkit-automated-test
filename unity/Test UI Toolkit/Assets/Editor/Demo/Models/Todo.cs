using System;

namespace Editor.Demo.Models
{
    public class Todo
    {
        public string Task { get; }

        public bool Complete { get; private set; }

        public Todo(string task, bool complete)
        {
            Task = task;
            Complete = complete;
        }
        public void NowComplete()
        {
            Complete = true;
        }
        public void NotComplete()
        {
            Complete = false;
        }
        public override bool Equals(object obj)
        {
            if (obj is Todo todo)
            {
                return Task == todo.Task && Complete == todo.Complete;
            }
            throw new InvalidCastException("obj is not a Todo");
        }
    }
}
