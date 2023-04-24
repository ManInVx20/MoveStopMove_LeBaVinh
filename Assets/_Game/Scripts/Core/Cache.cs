using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider, Character> characterDict = new Dictionary<Collider, Character>();
    private static Dictionary<Collider, Obstacle> obstacleDict = new Dictionary<Collider,Obstacle>();

    public static bool TryGetCharacter(Collider collider, out Character character)
    {
        if (characterDict.TryGetValue(collider, out character))
        {
            return true;
        }
        else if (collider.TryGetComponent<Character>(out character))
        {
            characterDict[collider] = character;

            return true;
        }

        character = null;

        return false;
    }

    public static bool TryGetObstacle(Collider collider, out Obstacle obstacle)
    {
        if (obstacleDict.TryGetValue(collider, out obstacle))
        {
            return true;
        }
        else if (collider.TryGetComponent<Obstacle>(out obstacle))
        {
            obstacleDict[collider] = obstacle;

            return true;
        }

        obstacle = null;

        return false;
    }
}
