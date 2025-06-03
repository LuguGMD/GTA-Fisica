using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private float deathCanvasDelay = 1;

    private void Start()
    {
        ActionsManager.Instance.onPlayerDeath += StartDeathSequence;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerDeath -= StartDeathSequence;
    }

    public void StartDeathSequence()
    {
        Invoke(nameof(CreateDeathCanvas), deathCanvasDelay);
    }

    public void CreateDeathCanvas()
    {
        Instantiate(deathCanvas);
    }
}
