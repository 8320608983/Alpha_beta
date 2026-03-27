using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    private List<CardScript> activeCards = new List<CardScript>();
    private Queue<List<CardScript>> queue = new Queue<List<CardScript>>();

    private int totalPairs;
    private int matchedPairs;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SetTotalPairs(int total)
    {
        totalPairs = total;
        matchedPairs = 0;
    }

    public void RegisterCard(CardScript card)
    {
        activeCards.Add(card);

        if (activeCards.Count >= 2)
        {
            List<CardScript> pair = new List<CardScript>()
            {
                activeCards[0],
                activeCards[1]
            };

            queue.Enqueue(pair);

            activeCards.RemoveAt(0);
            activeCards.RemoveAt(0);


            StartCoroutine(ProcessQueue());
        }
    }

    IEnumerator ProcessQueue()
    {
        while (queue.Count > 0)
        {
            var pair = queue.Dequeue();

            yield return new WaitForSeconds(0.4f);

            CardScript a = pair[0];
            CardScript b = pair[1];

            if (a.ID == b.ID)
            {
                a.MatchSuccess();
                b.MatchSuccess();

                matchedPairs++;
                 

            }
            else
            {
                a.FlipBack();
                b.FlipBack();

            }
        }

    }
}
