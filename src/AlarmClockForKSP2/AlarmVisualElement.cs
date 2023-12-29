using KSP.UI.Binding;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SpaceWarp.API.Assets;

namespace AlarmClockForKSP2
{
    public class AlarmVisualElement : VisualElement
    {
        public AlarmVisualElement()
        {
            var root = AssetManager.GetAsset<VisualTreeAsset>($"alarmclockforksp2/" + "alarmclock-resources/UI/AlarmEntry.uxml").CloneTree();

            Add(root);
        }

    }
}
