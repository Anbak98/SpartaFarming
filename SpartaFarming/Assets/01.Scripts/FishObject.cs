using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishObject : MonoBehaviour
{
    public Vector3 PowerDirection =>
        (NextMovePosition - transform.position).normalized;

    public int Stamina
    {
        get => _stamina;
        set
        {
            _stamina = value;
            if (_stamina < 0)
                _stamina = 0;
        }
    }

    #region Life Cycle
    private Vector3 originPosition;
    private Vector3 NextMovePosition;

    private float fishSpeed = 0.001f;
    private float timeAccel = 1f;
    private float elaspedTime = 0;
    private float timeInterval = 1;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elaspedTime += Time.deltaTime * timeAccel;

        if (elaspedTime > timeInterval)
        {
            NextMovePosition = originPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            elaspedTime = 0;
        }

        transform.position = Vector3.Lerp(transform.position, NextMovePosition, fishSpeed);
    }
    #endregion

    #region Private
    private int _stamina = 100;
    #endregion
}
