using System.Collections;
using System.Collections.Generic;
using Editor.Demo.Models;
using UIToolkitExtensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Editor.Demo.Controllers.Runtime
{
    public class TodoAppControllerTests
    {
        private static VisualElement FakeTodoAppView()
        {
            var containerVisElem = new VisualElement();
            containerVisElem.Add(new VisualElement
            {
                name = "todo-list_container"
            });
            containerVisElem.Add(new TextField
            {
                name = "user-input_todo-task"
            });
            containerVisElem.Add(new Toggle
            {
                name = "user-input_todo-is-complete"
            });
            containerVisElem.Add(new Button
            {
                name = "user-action_add-new-todo"
            });
            containerVisElem.Add(new Button
            {
                name = "user-action_remove-todo"
            });
            return containerVisElem;
        }
        [UnityTest]
        public IEnumerator TodoAppController_CanAcceptValidTodoFromUI()
        {
            var view = FakeTodoAppView();
            var gameObject = new GameObject();
            var panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            var uiDocument = gameObject.AddComponent<UIDocument>();
            uiDocument.rootVisualElement.Add(view);
            uiDocument.panelSettings = panelSettings;
            var sut = new TodoAppController(uiDocument.rootVisualElement);
            var todoAdded = false;
            sut.TodoAdded += _ => todoAdded = true;
            yield return null;

            var button = uiDocument.rootVisualElement.Q<Button>("user-action_add-new-todo");
            var textField = uiDocument.rootVisualElement.Q<TextField>("user-input_todo-task");
            textField.value = "fake task details";

            button.SimulateClick(new ClickEvent());
            yield return null;

            Assert.IsTrue(todoAdded);

            // Cleanup
            Object.DestroyImmediate(gameObject);
            Object.DestroyImmediate(panelSettings);
        }
        [UnityTest]
        public IEnumerator TodoAppController_CanRemoveTodoFromUI()
        {
            var view = FakeTodoAppView();
            var gameObject = new GameObject();
            var panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            var uiDocument = gameObject.AddComponent<UIDocument>();
            var fakeTodo = new Todo("fake task", false);
            var fakeTodos = new List<Todo> { fakeTodo };
            uiDocument.rootVisualElement.Add(view);
            uiDocument.panelSettings = panelSettings;
            var sut = new TodoAppController(uiDocument.rootVisualElement, fakeTodos);
            Todo todoRemoved = null;
            sut.TodoRemoved += todo => todoRemoved = todo;
            yield return null;

            var button = uiDocument.rootVisualElement.Q<Button>("user-action_remove-todo");
            button.SimulateClick(new ClickEvent());
            yield return null;

            Assert.IsNotNull(todoRemoved);
            Assert.AreEqual(fakeTodo, todoRemoved);

            // Cleanup
            Object.DestroyImmediate(gameObject);
            Object.DestroyImmediate(panelSettings);
        }
    }
}
