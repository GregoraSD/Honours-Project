using UnityEngine;

public class MemoirDisplayText : MonoBehaviour
{
    [SerializeField] private MemoirManager memoirManager;
    [SerializeField] private TMPro.TextMeshProUGUI textGUI;

    private void Update()
    {
        textGUI.text = "You discovered " + MemoirManager.FoundMemoirs + "/" + MemoirManager.MemoirCount + " memoirs.";
    }
}