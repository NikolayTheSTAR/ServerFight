using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;

namespace TheSTAR.GUI
{
    public abstract class GuiScreen : GuiObject, IComparable<GuiScreen>, IComparableType<GuiScreen>
    {
        [SerializeField] private bool pause;
        [HideInInspector] public bool useUniversalElements;
        [HideInInspector] public UnityDictionary<int, bool> universalElementsSettings = new();

        public List<int> UsedEniversalElementsIndexes
        {
            get
            {
                List<int> result = new();

                if (!useUniversalElements) return result;

                foreach (var pair in universalElementsSettings.KeyValues)
                {
                    if (pair.Value) result.Add(pair.Key);
                }

                return result;
            }
        }

        [HideInInspector] public bool useHardCounter;
        [HideInInspector] public bool useSoftCounter;

        public bool Pause => pause;

        public int CompareTo(GuiScreen other) => ToString().CompareTo(other.ToString());
        public int CompareToType<T>() where T : GuiScreen => ToString().CompareTo(typeof(T).ToString());

    }
}