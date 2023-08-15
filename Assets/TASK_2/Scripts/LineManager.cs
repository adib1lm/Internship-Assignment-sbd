using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private EdgeCollider2D _colider;

    private float _smoothness;
    private readonly List<Vector2> _positions = new List<Vector2>();
    public void setSmoothness(float _val)
    {
        _smoothness = _val;

        //removing the offset of edge colider
        _colider.transform.position -= transform.position;
    }
    public void setPosition(Vector2 _position)
    {
        if (!canDraw(_position)) return;

        _positions.Add(_position);

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _position);

        _colider.points = _positions.ToArray();
    }
    private bool canDraw(Vector2 _position)
    {
        if (_lineRenderer.positionCount == 0) return true;
        
        float _dist = Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), _position);
        return _dist > _smoothness;
    }
}
