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
     * - enemies
     * - doorState
     * - TODO spawnpoint (of player)
     */
    
    public static int life = 3;
    public static bool doorIsOpen;
    public static int numberOfKeys;
    public static int numberOfEnemies;
    private static GameObject gitter;

    public static void decreaseNeededKeys()
    {
        numberOfKeys--;
        if(numberOfKeys <= 0)
        {
            doorIsOpen = true;
            FindFirstObjectByType<AudioManager>().Play("clear_level");
        }
    }

    /*
     * TODO
     * - write multiple events:
     *  - player looses life
     *  - advance level
     *  - restart level
     *  - move back level
     */


}
