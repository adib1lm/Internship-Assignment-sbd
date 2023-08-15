using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{

    [Range(0.05f, 1f)]
    [SerializeField] private float _smoothness = 0.1f;
    [SerializeField] private Camera _mainCam;
    [SerializeField] private LineManager _linePrefab;
    [SerializeField] private GameController _gameController;

    private Vector2 _mouseWorldPos;
    private LineManager _currentLine;

    void Update()
    {
        //update mouse world pos//
        _mouseWorldPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (_gameController.GameState == gameState.inGame)
            getInput();
    }

    private void getInput()
    {
        
        //init lin on mouse down//
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentLine != null)
                destroyLine(_currentLine);

            //spawning a new line renderer//
            _currentLine = Instantiate(_linePrefab, _mouseWorldPos, Quaternion.identity);
            _currentLine.setSmoothness(_smoothness);
        }

        if (_currentLine == null)
            return;

        //while mouse is clicked//
        if (Input.GetMouseButton(0))
        {
            if (_currentLine == null) return;
            _currentLine.setPosition(_mouseWorldPos);
        }
        //when click finishes//
        if (Input.GetMouseButtonUp(0))
        {
            destroyLine(_currentLine);
            _gameController.endGame();
        }
    }

    private void destroyLine(LineManager _line)
    {
        Destroy(_line.gameObject);
    }
}
