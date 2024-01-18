using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /*
     * attributes
     * - keys
     * - time
     * - player lifes
     * - enemies?
     * - doorState?
     * 
     */
    
    public static int life = 3;
    public static bool doorIsOpen;
    public static int numberOfKeys;
    public static int numberOfEnemies;

    public static void decreaseNeededKeys()
    {
        numberOfKeys--;
        if(numberOfKeys <= 0)
        {
            doorIsOpen = true;
        }
    }
}
