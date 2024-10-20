using System.Threading.Tasks;

public interface IGameService
{
    void InitGame();
    Task LoadGame();
    void StartGame();
}