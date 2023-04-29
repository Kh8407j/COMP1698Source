// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace methods
{
    /* KH - This is a simple script I made for storing a collection of math-related methods that may be useful. */

    public class Math
    {
        // KH - Returns either true or false by using a random percentage chance from 0 to 100.
        public bool PercentageChance(float chance)
        {
            // KH - Pick a random number from zero to one hundred, see if it fits in the requested percentage chance.
            float randomVal = Random.Range(0f, 100f);
            if (randomVal > 100f - chance)
                return true;
            else
                return false;
        }
    }
}