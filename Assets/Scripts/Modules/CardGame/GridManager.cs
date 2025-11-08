using System.Collections.Generic;
using UnityEngine;

public class GridManager : List<List<GridField>>
{
    public GridManager(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Add(new List<GridField>());
            for (int j = 0; j < size; j++)
            {
                this[i].Add(null);
            }
        }
    }

    public void AddField(GridField field)
    {
        int x = Mathf.RoundToInt(field.transform.position.x);
        int z = Mathf.RoundToInt(field.transform.position.z);

        if(x > Count || z > this[x].Count)
        {
            Debug.LogWarning($"Index out of bounds: {x}/{z}");
            return;
        }

        this[x][z] = field;
    }

    public void EnableSelection(bool value)
    {
        foreach (List<GridField> fieldList in this)
        {
            foreach(GridField field in fieldList)
            {
                if(field == null)
                {
                    continue;
                }

                field.SetSelectionVisibility(value);
            }
        }
    }

    public GridField[] GetNeighbours (GridField origin)
    {
        int x = Mathf.RoundToInt(origin.transform.position.x);
        int z = Mathf.RoundToInt(origin.transform.position.z);

        List<GridField> result = new List<GridField>();

        for (int ix = x - 1; ix <= x + 1; ix++)
        {
            for (int iz = z - 1; iz <= z + 1; iz++)
            {
                if(ix == x && iz == z)
                {
                    continue;
                }

                if(TryGet(ix, iz , out GridField neighbour))
                {
                    result.Add(neighbour);
                }
            }
        }

        return result.ToArray();
    }

    public GridField[] GetRow(GridField origin)
    {
        return this[Mathf.RoundToInt(origin.transform.position.x)].ToArray();
    }

    public GridField[] GetColumn(GridField origin)
    {
        List<GridField> result = new List<GridField>();
        int z = Mathf.RoundToInt(origin.transform.position.z);

        for (int i = 0; i < Count; i++)
        {
            if(TryGet(i, z, out GridField neighbour))
            {
                result.Add(neighbour);
            }
        }

        return result.ToArray();
    }

    private bool TryGet(int x, int z, out GridField field)
    {
        if (x > Count || z > this[x].Count)
        {
            field = null;
            return false;
        }

        field = this[x][z];
        return field != null;
    }
}