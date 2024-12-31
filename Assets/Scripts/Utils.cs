using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Extension methods for various classes
/// </summary>
public static class Utils
{
    /// <summary>
    /// Returns the opposite direction up -> down, down -> up, left -> right, right -> left
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static string ReverseDirection(this string direction)
    {
        switch (direction)
        {
            case "up":
                return "down";
            case "down":
                return "up";
            case "left":
                return "right";
            case "right":
                return "left";
            default:
                return "";
        }
    }
}
