using System;
using UnityEngine;
using Zenject;
using TheSTAR.GUI;
using TheSTAR.Data;

/// <summary>
/// Биндим в контексте сцены Game
/// </summary>
public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private GameWorld worldPrefab;
    [SerializeField] private GameManager gameManagerPrefab;

    [Header("GUI")]
    [SerializeField] private GuiController guiControllerPrefab;
    [SerializeField] private GuiScreen[] screenPrefabs;
    [SerializeField] private GuiUniversalElement[] universalElementPrefabs;

    private GuiController gui;

    public override void InstallBindings()
    {
        Container.Bind<DataController>().AsSingle();

        InstallGuiContainers();
        
        // world
        var world = Container.InstantiatePrefabForComponent<GameWorld>(worldPrefab, worldPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GameWorld>().FromInstance(world).AsSingle();

        // network
        Container.Bind<IGameServer>().To<TestGameServer>().AsSingle(); // тут можно будет переключать сервер с тестового на реальный
        Container.Bind<IGameClient>().To<MobileGameClient>().AsSingle(); // тут можно будет менять платформу клиента на необходимую (мобайл, PC и прочее)

        var gameManager = Container.InstantiatePrefabForComponent<GameManager>(gameManagerPrefab);
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();

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