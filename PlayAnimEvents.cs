using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimEvents : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();

        if (player == null)
        {
            Debug.Log("Player is null on animator");
        }

    }

    private void AnimationTrigger()
    {
        player.AttackOver();
    }
}
