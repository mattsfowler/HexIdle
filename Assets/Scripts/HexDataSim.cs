using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDataSim : MonoBehaviour
{
    // These are only public to make them editable inside Unity. It is not recommended to change these at run-time!
    public float defaultFoodPerTern = 5;
    public float defaultMaxFood = 20;

    private float currentFood = 0;
    private List<TileModifier> allModifiers;
    private Vector2 myGridPosition;

    void Start()
    {
        //tile starts with no modifiers
        allModifiers = new List<TileModifier>();
        currentFood = 0;
    }


    public void SetCoords(int col, int row)
    {
        myGridPosition = new Vector2(col, row);
    }

    public Vector2 GetCoords()
    {
        return myGridPosition;
    }

    public float GetCurrentFood()
    {
        return currentFood;
    }

    public float CalculateFoodThisTurn()
    {
        return defaultFoodPerTern;
    }

    public float CalculateMaxFood()
    {
        return defaultMaxFood;
    }


    // Used to indirectly modify tile values through tile modifiers. 'name' parameter must be unique.
    public bool AddTileModifier(TileModifier newModifier, bool overwrite=false)
    {
        for (int index = 0; index < allModifiers.Count; index++)
        {
            if (allModifiers[index].name == newModifier.name)
            {
                if (overwrite == true)
                {
                    allModifiers[index] = newModifier;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        allModifiers.Add(newModifier);
        return true;
    }

    // Removes an active modifier by name. Returns false if no modifier with that name exists.
    public bool RemoveTileModifier(string name)
    {
        for (int index = 0; index < allModifiers.Count; index++)
        {
            if (allModifiers[index].name == name)
            {
                allModifiers.RemoveAt(index);
                return true;
            }
        }
        return false;
    }

    public TileModifier[] GetModifiers()
    {
        TileModifier[] output = new TileModifier[allModifiers.Count];
        allModifiers.CopyTo(output);
        return output;
    }

    public void AdvanceTurn()
    {
        currentFood += CalculateFoodThisTurn(); //add food to tile
        currentFood = Mathf.Min(currentFood, CalculateMaxFood()); //cap food to limit
    }
}
