using UnityEngine;

public class GameVars
{
    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.yellow;
        }

        return Color.black;
    }
}
