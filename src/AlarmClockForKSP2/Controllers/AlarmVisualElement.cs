using UnityEngine.UIElements;
using SpaceWarp.API.Assets;

namespace AlarmClockForKSP2
{
    public class AlarmVisualElement : VisualElement
    {
        public AlarmVisualElement()
        {
            TemplateContainer root = AssetManager.GetAsset<VisualTreeAsset>($"alarmclockforksp2/" + "alarmclock-resources/UI/AlarmEntry.uxml").CloneTree();

            Add(root);
        }

    }
}
