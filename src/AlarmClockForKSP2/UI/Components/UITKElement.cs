using SpaceWarp.API.Assets;
using UnityEngine.UIElements;

namespace AlarmClockForKSP2
{
    public class UITKElement : VisualElement
    {
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
        public UITKElement(string assetPath)
        {
            if (AssetManager.GetAsset<VisualTreeAsset>($"{AlarmClockForKSP2Plugin.ModGuid}/{assetPath}").CloneTree() is TemplateContainer template) Add(template);
        }
    }
}
