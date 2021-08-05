using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnParticleSystemStopped()
    {
        gameController.OnParticlesAnimationDone();
        Destroy(gameObject);
    }
}
