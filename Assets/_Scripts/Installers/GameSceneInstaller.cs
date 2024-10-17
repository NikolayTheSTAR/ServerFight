using System;
using UnityEngine;
using Zenject;
using TheSTAR.GUI;

/// <summary>
/// Биндим в контексте сцены Game
/// </summary>
public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private GameWorld worldPrefab;

    [Header("GUI")]
    [SerializeField] private GuiController guiControllerPrefab;
    [SerializeField] private GuiScreen[] screenPrefabs;
    [SerializeField] private GuiUniversalElement[] universalElementPrefabs;

    private GuiController gui;

    public override void InstallBindings()
    {
        InstallGuiContainers();
        
        // world
        var world = Container.InstantiatePrefabForComponent<GameWorld>(worldPrefab, worldPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GameWorld>().FromInstance(world).AsSingle();

        // gui
        InstallGuiScreens();
    }

    private void InstallGuiContainers()
    {
        gui = Container.InstantiatePrefabForComponent<GuiController>(guiControllerPrefab, guiControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GuiController>().FromInstance(gui).AsSingle();
    }

    private void InstallGuiScreens()
    {
        GuiScreen[] createdScreens = new GuiScreen[screenPrefabs.Length];
        GuiScreen screen;
        for (int i = 0; i < screenPrefabs.Length; i++)
        {
            screen = Container.InstantiatePrefabForComponent<GuiScreen>(screenPrefabs[i], gui.ScreensContainer.position, Quaternion.identity, gui.ScreensContainer);
            createdScreens[i] = screen;
        }

        GuiUniversalElement[] createdUniversalElements = new GuiUniversalElement[universalElementPrefabs.Length];
        GuiUniversalElement ue;
        for (int i = 0; i < universalElementPrefabs.Length; i++)
        {
            ue = Container.InstantiatePrefabForComponent<GuiUniversalElement>(universalElementPrefabs[i], gui.UniversalElementsContainer.position, Quaternion.identity, gui.UniversalElementsContainer);
            createdUniversalElements[i] = ue;
        }

        gui.Set(createdScreens, createdUniversalElements);
    }

    [ContextMenu("Sort")]
    private void Sort()
    {
        Array.Sort(screenPrefabs);
        Array.Sort(universalElementPrefabs);
    }
}