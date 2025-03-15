// =======================================================================================
// Maintained by bobatea#9400 on Discord
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: 
 
// * Leave a star on my Github Repo.....: https://github.com/breehuynh/Bree-mmorpg-tools
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Equipmentizer : MonoBehaviour
{
    [Tooltip("Here goes the SkinnedMeshRenderer you want to target")]
    public GameObject target;

    public GameObject main;

    private void Start()
    {
        /*SpriteRenderer targetRenderer = target.GetComponent<SpriteRenderer>();
        SubAnimation myRenderer = main.GetComponent<SubAnimation>();
        for(int i = 0; i < myRenderer.spritesToAnimate.Count; i++)
        {
            targetRenderer.sprite = myRenderer.spritesToAnimate[i];
        }
        */
        
    }
}
