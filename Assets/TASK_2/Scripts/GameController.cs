using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public gameState GameState = gameState.idle;


    [SerializeField] [Range(5, 10)] private int circleCount = 5; 
    [SerializeField] private Circle _circlePrefab;
    [SerializeField] private Camera _mainCam;
    
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private GameObject bt_Start;
    [SerializeField] private GameObject bt_Restart;

    private List<Circle> _circleList = new List<Circle>();
    public void startGame()
    {
        
        spawnCircle();
        GameState = gameState.inGame;

        if (bt_Start.activeInHierarchy)
            bt_Start.gameObject.SetActive(false);
        if (bt_Restart.activeInHierarchy)
            bt_Restart.gameObject.SetActive(false);
        _menuCanvas.gameObject.SetActive(false);
    }

    private void clearScreen()
    {
        foreach (var circle in _circleList)
            if(circle != null)
                Destroy(circle.gameObject);
    }

    private void spawnCircle()
    {
        _circleList.Clear();
        float _circleOffset = _circlePrefab.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        for (int i = 0; i < circleCount; i++)
        {
            Vector3 screenPosition = _mainCam.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width ), Random.Range(0, Screen.height), _mainCam.farClipPlane / 2));
            screenPosition = new Vector3(screenPosition.x<0? screenPosition.x + 1:screenPosition.x - 1, 
                                         screenPosition.y<0? screenPosition.y + 1: screenPosition.y - 1,
                                         screenPosition.z);
            var circle = Instantiate(_circlePrefab, screenPosition, Quaternion.identity);
            _circleList.Add(circle);
        }
    }

    public void endGame()
    {
        evaluateResult();
        GameState = gameState.end;
        _menuCanvas.gameObject.SetActive(true);
        bt_Restart.gameObject.SetActive(true);

    }

    private void evaluateResult()
    {
        foreach(var circle in _circleList)
        {
            circle.checkCollision();
        }
    }

    public void restartGame()
    {
        clearScreen();
        startGame();
    }
}

public enum gameState
{   
    idle,
    inGame,
    end
}
