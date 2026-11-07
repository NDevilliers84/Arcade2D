using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

    [CustomPropertyDrawer(typeof(Trigger))]
    public class TriggerDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(
            SerializedProperty property)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.alignItems = Align.Center;
            root.style.height = 22;

            // Property label
            var label = new Label(property.displayName);
            label.style.flexGrow = 1;
            root.Add(label);

            // LED
            var led = new VisualElement();
            led.style.width = 9;
            led.style.height = 9;
            led.style.borderTopLeftRadius = 5;
            led.style.borderTopRightRadius = 5;
            led.style.borderBottomLeftRadius = 5;
            led.style.borderBottomRightRadius = 5;
            led.style.marginRight = 7;
            root.Add(led);

            // Status
            var status = new Label();
            status.style.width = 65;
            status.style.unityFontStyleAndWeight = FontStyle.Bold;
            status.style.fontSize = 10;
            root.Add(status);

            // Remaining time
            var timer = new Label();
            timer.style.width = 45;
            timer.style.unityTextAlign = TextAnchor.MiddleRight;
            timer.style.color = Color.gray;
            root.Add(timer);

            var endTime = property.FindPropertyRelative("_endTime");

            void Refresh()
            {
                if (endTime == null) return;

                property.serializedObject.UpdateIfRequiredOrScript();

                float remaining = Application.isPlaying
                    ? Mathf.Max(0f, endTime.floatValue - Time.time)
                    : 0f;

                bool active = remaining > 0f;

                led.style.backgroundColor = active
                    ? new Color(0.22f, 0.75f, 0.42f)
                    : new Color(0.4f, 0.4f, 0.4f);

                status.text = active ? "ACTIVE" : "INACTIVE";

                status.style.color = active
                    ? new Color(0.35f, 0.85f, 0.55f)
                    : Color.gray;

                timer.text = active ? $"{remaining:0.00}s" : "";
            }

            root.schedule.Execute(Refresh).Every(50);
            Refresh();

            return root;
        }
    }