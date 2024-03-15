using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string QUEST_ID;
    public bool hasBeenTaken;

    private void Start()
    {
        QuestManager.instance.AddCollectible(this);
        gameObject.SetActive(false);
    }

    public void PickUp()
    {
        hasBeenTaken = true;
        gameObject.SetActive(false);
    }
}