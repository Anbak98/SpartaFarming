using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Controller;

    private void Awake()
    {
        GameManager.Instance.Player = this;
        Controller = GetComponent<PlayerController>();
    }
}
