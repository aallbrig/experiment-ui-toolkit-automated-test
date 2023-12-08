using System;
using System.Collections.Generic;
using Editor.Demo.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Demo.Controllers
{
    public class TodoAppController
    {
        private readonly Toggle _todoCompleteToggle;
        private readonly VisualElement _todoListContainer;
        private readonly List<Todo> _todos = new();
        private readonly TextField _todoTaskInput;
        public Action<Todo> TodoAdded;
        public Action<Todo> TodoRemoved;
        public TodoAppController(VisualElement view, IEnumerable<Todo> todos = null)
        {
            if (view == null) throw new ArgumentNullException();

            var addTodoButton = view.Q<Button>("user-action_add-new-todo");
            if (addTodoButton == null) throw new NullReferenceException("Could not find user-action_add-todo button");
            _todoTaskInput = view.Q<TextField>("user-input_todo-task");
            if (_todoTaskInput == null) throw new NullReferenceException("Could not find user-input_todo-task text field");
            _todoCompleteToggle = view.Q<Toggle>("user-input_todo-is-complete");
            if (_todoCompleteToggle == null)
                throw new NullReferenceException("Could not find user-input_todo-is-complete toggle");
            _todoListContainer = view.Q<VisualElement>("todo-list_container");
            if (_todoListContainer == null)
                throw new NullReferenceException("Could not find todo-list_container visual element");

            if (todos != null)
                foreach (var todo in todos)
                    AddTodo(todo);

            addTodoButton.clickable.clickedWithEventInfo += OnAddTodoButtonClicked;
        }
        private void OnAddTodoButtonClicked(EventBase evt)
        {
            if (_todoTaskInput.value == string.Empty)
            {
                Debug.LogError("user did not enter a task");
                return;
            }
            AddTodo(new Todo(_todoTaskInput.value, _todoCompleteToggle.value));
        }
        private bool AddTodo(Todo todo)
        {
            if (!_todos.Contains(todo))
            {
                _todos.Add(todo);
                TodoAdded?.Invoke(todo);
                var todoView = new VisualElement();
                todoView.Add(new Label { text = $"{todo.Task}" });
                todoView.Add(new Toggle { value = todo.Complete });
                var removeButton = new Button
                    { name = "user-action_remove-todo", text = "Remove this todo", userData = todo };
                removeButton.clickable.clickedWithEventInfo += evt =>
                {
                    RemoveTodo(todo);
                    todoView.parent.Remove(todoView);
                };
                todoView.Add(removeButton);
                _todoListContainer.Add(todoView);
                return true;
            }
            return false;
        }
        private bool RemoveTodo(Todo todo)
        {
            if (!_todos.Contains(todo))
                return false;
            _todos.Remove(todo);
            TodoRemoved?.Invoke(todo);
            return true;
        }
        public void ToggleTodo(Todo todo)
        {
            if (!_todos.Contains(todo))
                return;
            if (todo.Complete) todo.NotComplete();
            else todo.NotComplete();
        }
    }
}
