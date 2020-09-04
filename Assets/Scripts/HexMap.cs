using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //hex dimensions pre-calculation
        hexSize = 1.0f;
        hexWidth = OFFSET_CONST * 2 * hexSize;
        hexHeight = hexSize * 2.0f;
        horDistance = hexWidth;
        verDistance = hexHeight * 0.75f;
        trueMapWidth = horDistance * mapWidth;

        GenerateHexes();
    }

    public GameObject HexTemplate;
    public int mapWidth;
    public int mapHeight;
    public GameObject CameraObject;

    private readonly float OFFSET_CONST = Mathf.Sqrt(3) / 2;
    private List<GameObject> hexList;
    private float hexSize; //can be made public if variable hex size is required (I don't see why rn tho)
    private float hexWidth;
    private float hexHeight;
    private float horDistance;
    private float verDistance;
    private float trueMapWidth;

    void Update()
    {
        UpdateHexPositions(); //TODO: should only update if the camera updates
    }

    private void GenerateHexes()
    {
        //make sure we've got a phresh list
        hexList = new List<GameObject>();

        for (int col = 0; col < mapWidth; col++)
        {
            for (int row = 0; row < mapHeight; row++) 
            {
                GameObject newHex = Instantiate(HexTemplate, CalculateHexPosition(col, row), transform.rotation, transform);
                newHex.GetComponent<HexDataSim>().SetCoords(col, row); //just lets the hex itself know where it is on the map
                newHex.name = "Hex_" + col.ToString() + "_" + row.ToString();
                hexList.Add(newHex);
            }
        }
    }

    private void UpdateHexPositions()
    {
        float cameraX = CameraObject.transform.position.x;
        for (int col = 0; col < mapWidth; col++)
        {
            for (int row = 0; row < mapHeight; row++)
            {
                GameObject hex = GetHexAt(col, row);
                hex.transform.position = CalculateHexPosition(col, row);
            }
        }
    }

    private Vector3 AxialToCube(Vector2 hex)
    {
        float x = hex.x;
        float z = hex.y;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    private Vector2 CubeToAxial(Vector3 cube)
    {
        float q = cube.x;
        float r = cube.z;
        return new Vector2(q, r);
    }

    private Vector3 CubeRound(Vector3 cube)
    {
        float rx = Mathf.Round(cube.x);
        float ry = Mathf.Round(cube.y);
        float rz = Mathf.Round(cube.z);

        float x_diff = Mathf.Abs(rx - cube.x);
        float y_diff = Mathf.Abs(ry - cube.y);
        float z_diff = Mathf.Abs(rz - cube.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector3(rx, ry, rz);
    }

    private Vector2 HexRound(Vector2 hex)
    {
        return CubeToAxial(CubeRound(AxialToCube(hex)));
    }


    public Vector3 CalculateHexPosition(int q, int r)
    {   
        //explination - https://www.redblobgames.com/grids/hexagons/#hex-to-pixel
        float x = hexSize * ((OFFSET_CONST * 2 * q) + (OFFSET_CONST * r));
        float y = hexSize * 1.5f * r;
        float camera_x = CameraObject.transform.position.x;

        //do we need to wrap this hex?
        float howManyWidthsFromCamera = (x - camera_x) / trueMapWidth;
        if (Mathf.Abs(howManyWidthsFromCamera) <= 0.5f)
            return new Vector3(x, 0.0f, y); //nope we gud

        //yep, this wack (ngl I don't really get this lol, taken from Quill18's vid ep2)
        if (howManyWidthsFromCamera > 0)
            howManyWidthsFromCamera += 0.5f;
        else
            howManyWidthsFromCamera -= 0.5f;
        x -= (int)howManyWidthsFromCamera * trueMapWidth;

        return new Vector3(x, 0.0f, y);
    }

    public GameObject GetHexAt(int col, int row)
    {
        int index = col + (mapWidth * row);
        if (hexList.Count <= index)
            return null;
        else
            return hexList[index];
    }

    public Vector2Int GetHexPositionFromWorldspace(float x, float z)
    {
        float fractional_q = ((Mathf.Sqrt(3) / 3 * x) - (1/3 * z)) / hexSize;
        float fractional_r = (2/3 * z) / hexSize;
        Vector2 hex = HexRound(new Vector2(fractional_q, fractional_r));
        return new Vector2Int((int)hex.x, (int)hex.y);
    }

    public void NextTurn()
    {
        foreach (GameObject hex in hexList)
        {
            hex.GetComponent<HexDataSim>().AdvanceTurn();
        }
    }
}
