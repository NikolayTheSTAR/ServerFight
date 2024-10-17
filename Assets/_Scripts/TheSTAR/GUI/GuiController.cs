using System;
using UnityEngine;
using TheSTAR.Utility;
using Zenject;

namespace TheSTAR.GUI
{
    public sealed class GuiController : MonoBehaviour
    {
        #region Inspector

        private GuiScreen[] screens = new GuiScreen[0];
        private GuiUniversalElement[] universalElements = new GuiUniversalElement[0];
        private GuiScreen mainScreen;

        [SerializeField] private Transform screensContainer;
        [SerializeField] private Transform universalElementsContainer;
        [SerializeField] private bool deactivateOtherScreensByStart = true;
        [SerializeField] private bool showMainScreenByStart = true;

        #endregion // Inspector

        private GuiScreen _currentScreen;
        public GuiScreen CurrentScreen => _currentScreen;

        public Transform ScreensContainer => screensContainer;
        public Transform UniversalElementsContainer => universalElementsContainer;

        private readonly Type mainScreenType = typeof(GameScreen);
        private Type[] allUniversalElementTypes;

        [Inject]
        private void Construct()
        {
            allUniversalElementTypes = ReflectiveEnumerator.GetEnumerableOfType<GuiUniversalElement>().ToArray();

            SortScreens();
        }

        public void ShowMainScreen()
        {
            Show(mainScreen, true);
        }

        public void Show<TScreen>(bool closeCurrentScreen = true, Action endAction = null) where TScreen : GuiScreen
        {
            GuiScreen screen = FindScreen<TScreen>();
            Show(screen, closeCurrentScreen, endAction);
        }

        public void Show<TScreen>(TScreen screen, bool closeCurrentScreen = true, Action endAction = null, bool skipShowAnim = false) where TScreen : GuiScreen
        {
            if (!screen) return;
            if (closeCurrentScreen && _currentScreen) _currentScreen.Hide();
            
            screen.Show(endAction, skipShowAnim);
            _currentScreen = screen;

            Time.timeScale = screen.Pause ? 0 : 1;

            var usedUeIndexes = _currentScreen.UsedEniversalElementsIndexes;

            for (int i = 0; i < allUniversalElementTypes.Length; i++)
            {
                Type ueType = allUniversalElementTypes[i];
                UpdateUniversalPanel(ueType, usedUeIndexes.Contains(i));
            }

            bool UpdateUniversalPanel(Type universalElementType, bool needShow)
            {
                var element = FindUniversalElement(universalElementType);
                if (needShow) element.Show();
                else element.Hide();

                return needShow;
            }
        }

        public void Show(Type screenType)
        {
            GuiScreen screen = FindScreen(screenType);
            Show(screen);
        }

        public GuiScreen FindScreen(Type screenType)
        {
            foreach (var screen in screens)
            {
                if (screen.GetType() == screenType) return screen;
            }

            return null;
        }

        public T FindScreen<T>() where T : GuiScreen
        {
            int index = ArrayUtility.FastFindElement<GuiScreen, T>(screens);

            if (index == -1)
            {
                Debug.LogError($"Not found screen {typeof(T)}");
                return null;
            }
            else return (T)(screens[index]);
        }

        public GuiUniversalElement FindUniversalElement(Type universalElementType)
        {
            foreach (var universalElement in universalElements)
            {
                if (universalElement.GetType() == universalElementType) return universalElement;
            }

            return null;
        }

        public T FindUniversalElement<T>() where T : GuiUniversalElement
        {
            int index = ArrayUtility.FastFindElement<GuiUniversalElement, T>(universalElements);

            if (index == -1)
            {
                Debug.LogError($"Not found universal element {typeof(T)}");
                return null;
            }
            else return (T)(universalElements[index]);
        }

        [ContextMenu("GetScreens")]
        private void GetScreens()
        {
            screens = GetComponentsInChildren<GuiScreen>(true);
            universalElements = GetComponentsInChildren<GuiUniversalElement>(true);
        }

        [ContextMenu("SortScreens")]
        private void SortScreens()
        {
            System.Array.Sort(screens);
            System.Array.Sort(universalElements);
        }

        public void Set(GuiScreen[] newScreens, GuiUniversalElement[] newUniversalElements)
        {
            this.screens = newScreens;
            this.universalElements = newUniversalElements;

            if (showMainScreenByStart && mainScreen != null) Show(mainScreen, false);

            foreach (var screen in screens)
            {
                if (screen == null) continue;
                if (deactivateOtherScreensByStart && screen.gameObject.activeSelf) screen.gameObject.SetActive(false);

                screen.Init();

                if (screen.GetType() == mainScreenType) mainScreen = screen;
            }

            foreach (var ue in universalElements)
            {
                if (ue == null) continue;
                if (deactivateOtherScreensByStart && ue.gameObject.activeSelf) ue.gameObject.SetActive(false);

                ue.Init();
            }
        }
    }
}