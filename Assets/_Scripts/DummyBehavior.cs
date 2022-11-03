using UnityEngine;

public class DummyBehavior : MonoBehaviour
{
    private Animator anim;

    public delegate void OnDummyHit();
    public static event OnDummyHit OnDummyHitEvent;

    private void Awake()
    {
        if(!(anim = GetComponent<Animator>()))
            throw new System.Exception("No animator found");
    }

    private void OnCollisionEnter(Collision collision)
    {
        anim.Play("pushed", 0, 0.0f);

        if (GameManager.Instance.GetGameState() == 0)
            OnDummyHitEvent?.Invoke(); //GameManager.Instance.SetGameState(1);
    }
}
