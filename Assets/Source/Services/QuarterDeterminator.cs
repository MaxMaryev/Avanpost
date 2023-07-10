using UnityEngine;

public class QuarterDeterminator
{
    public int GetQuarter(Vector2 direction)
    {
        int currentQuarter = 1;

        if (direction.x >= 0 && direction.y >= 0)
            currentQuarter = 1;
        else if (direction.x <= 0 && direction.y >= 0)
            currentQuarter = 2;
        else if (direction.x <= 0 && direction.y <= 0)
            currentQuarter = 3;
        else if (direction.x >= 0 && direction.y <= 0)
            currentQuarter = 4;

        return currentQuarter;
    }
}
