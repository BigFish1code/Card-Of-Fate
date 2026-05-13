using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu;

    private Room currentRoom; 
    private Vector2Int currentRoomVector;
    public ObjectEventSO updateRoomEvent;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        LoadMenu();
    }

    [Header(header: "广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = data as Room;

            var currentData = currentRoom.roomData;
            currentRoomVector = new(currentRoom.column, currentRoom.line);

            currentScene = currentData.sceneToLoad;
        }

        await UnloadSceneTask();
        await LoadSceneTask();

        afterRoomLoadedEvent.RaisEvent(currentRoom, this);
    }
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }
    private async Awaitable UnloadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    public async void LoadMap()
    {
        await UnloadSceneTask();
        if(currentRoomVector!= Vector2.one * -1)
        {
            updateRoomEvent.RaisEvent(currentRoomVector, this);
        }
        currentScene = map; 
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if(currentScene!= null)
            await UnloadSceneTask();
        currentScene = menu;
        await LoadSceneTask();
    }
}



