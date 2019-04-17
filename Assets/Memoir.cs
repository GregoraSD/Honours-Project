using UnityEngine;

public class Memoir : MonoBehaviour
{
    [SerializeField] private MemoirManager memoirManager;

    [SerializeField] private AudioSource memoirAudio;

    private bool found = false;

    public void Discover()
    {
        if (found) return;
        memoirManager.DiscoverMemoir(this);
        memoirAudio.Play();
        found = true;
    }
}