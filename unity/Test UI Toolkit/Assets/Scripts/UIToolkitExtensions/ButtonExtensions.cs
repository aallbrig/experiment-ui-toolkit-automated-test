using UnityEngine.UIElements;

namespace UIToolkitExtensions
{
    public static class ButtonExtensions
    {
        public static void SimulateClick(this Button button, EventBase evt) => button.clickable.SimulateSingleClick(evt);
    }
}
