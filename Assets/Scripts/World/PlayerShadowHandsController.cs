using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerShadowHandsController : MonoBehaviour
{
    public Light2D light2dController;
    public int bigHandAmount;
    public int mediumHandAmount;
    public int smallHandAmount;
    public RuntimeAnimatorController[] HandAnimators;
    public List<ShadowHandsController> BigShadowHands;
    public List<ShadowHandsController> MediumShadowHands;
    public List<ShadowHandsController> SmallShadowHands;
    private List<int> HandAnimatorIndexes;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure all hands are inactive
        BigShadowHands.ForEach(h => h.gameObject.SetActive(false));
        MediumShadowHands.ForEach(h => h.gameObject.SetActive(false));
        SmallShadowHands.ForEach(h => h.gameObject.SetActive(false));
        HandAnimatorIndexes = Enumerable.Range(0, HandAnimators.Length).ToList();
        GenerateRandomHands();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void GenerateRandomHands()
    {
        var existingHandPool = AddRandomHand(BigShadowHands, new List<ShadowHandsController>(), bigHandAmount);
        existingHandPool = AddRandomHand(MediumShadowHands, existingHandPool, bigHandAmount + mediumHandAmount);
        existingHandPool = AddRandomHand(SmallShadowHands, existingHandPool, bigHandAmount + mediumHandAmount + smallHandAmount);
    }

    private List<ShadowHandsController> AddRandomHand(List<ShadowHandsController> handsPool,
        List<ShadowHandsController> existingHandsPool, int poolLimit)
    {
        while(existingHandsPool.Count < poolLimit)
        {
            // Check if position is taken
            var hand = handsPool[Random.Range(0, handsPool.Count)];
            if (existingHandsPool.Any(h => h.Position == hand.Position)) continue;

            // Assign random hand
            var animator = hand.GetComponentInChildren<Animator>();
            var randIndex = Random.Range(0, HandAnimatorIndexes.Count);
            var handIndex = HandAnimatorIndexes[randIndex];
            animator.runtimeAnimatorController = HandAnimators[handIndex];
            HandAnimatorIndexes.RemoveAt(randIndex);

            // Enable object
            hand.gameObject.SetActive(true);
            existingHandsPool.Add(hand);
        }
        return existingHandsPool;
    }
}
