using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;
using Color = UnityEngine.Color;

/// Algoritmo de generacion tomado de http://toyscaos.tripod.com/maze_generator.html

public class LabyrinthGenerator : MonoBehaviour
{
    struct Cell
    {
        public byte N, S, E, O;

        public override string ToString()
        {
            return $"N:{N}; S:{S}; E:{E}; O:{O}";
        }
    }

    public int Padd { get; set; }
    public Random CurrentCell { get; set; }
    private Cell[,] Cell0;
    public Tuple<int, int, int, int> Generate(int LimH, int LimV, bool firstTime)
    {
        Stack stack1 = new Stack();
        Stack stack2 = new Stack();

        int totalCells, visitedCells, thisCell1, thisCell2;
        int oneOrMoreCell, otherCell;
        int m = 0, n = 0;

        for (int i = 0; i <= LimH; i += Padd)
            n++; //Horizontal

        for (int i = 0; i <= LimV; i += Padd)
            m++; //vertical

        totalCells = m * n;

        if (firstTime)
        {
            Cell0 = new Cell[m, n];

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    Cell0[i, j].O = 1;
                    Cell0[i, j].N = 1;
                    Cell0[i, j].E = 1;
                    Cell0[i, j].S = 1;

                    if (i == 0)
                        Cell0[i, j].N = 3;

                    if (j == 0)
                        Cell0[i, j].O = 3;

                    if (i == m - 1)
                        Cell0[i, j].S = 3;

                    if (j == n - 1)
                        Cell0[i, j].E = 3;
                }

            visitedCells = 1;
            thisCell1 = (int)CurrentCell.Next(0, m); //Random no incluye el maximo valor
            thisCell2 = (int)CurrentCell.Next(0, n);

            while (visitedCells < totalCells)
            {
                oneOrMoreCell = 0;

                if (Cell0[thisCell1, thisCell2].N == 1)
                    if (Cell0[thisCell1 - 1, thisCell2].N != 0 &&
                        Cell0[thisCell1 - 1, thisCell2].E != 0 &&
                        Cell0[thisCell1 - 1, thisCell2].O != 0 &&
                        Cell0[thisCell1 - 1, thisCell2].S != 0)
                        oneOrMoreCell++;

                if (Cell0[thisCell1, thisCell2].S == 1)
                    if (Cell0[thisCell1 + 1, thisCell2].S != 0 &&
                        Cell0[thisCell1 + 1, thisCell2].E != 0 &&
                        Cell0[thisCell1 + 1, thisCell2].O != 0 &&
                        Cell0[thisCell1 + 1, thisCell2].N != 0)
                        oneOrMoreCell++;

                if (Cell0[thisCell1, thisCell2].E == 1)
                    if (Cell0[thisCell1, thisCell2 + 1].N != 0 &&
                        Cell0[thisCell1, thisCell2 + 1].S != 0 &&
                        Cell0[thisCell1, thisCell2 + 1].E != 0 &&
                        Cell0[thisCell1, thisCell2 + 1].O != 0)
                        oneOrMoreCell++;

                if (Cell0[thisCell1, thisCell2].O == 1)
                    if (Cell0[thisCell1, thisCell2 - 1].N != 0 &&
                        Cell0[thisCell1, thisCell2 - 1].S != 0 &&
                        Cell0[thisCell1, thisCell2 - 1].O != 0 &&
                        Cell0[thisCell1, thisCell2 - 1].E != 0)
                        oneOrMoreCell++;

                if (oneOrMoreCell > 0) // hay al menos una celda vecina con todas sus paredes
                {
                    otherCell = selectCell(ref Cell0, thisCell1, thisCell2);
                    stack1.Push(thisCell1);
                    stack2.Push(thisCell2);

                    switch (otherCell)
                    {
                        case 1:
                            thisCell1--;
                            break;

                        case 2:
                            thisCell1++;
                            break;

                        case 3:
                            thisCell2++;
                            break;

                        case 4:
                            thisCell2--;
                            break;
                    }
                    visitedCells++;
                }
                else
                {

                    thisCell1 = (int)stack1.Pop();
                    thisCell2 = (int)stack2.Pop();
                }

            }
        }
        return Tuple.Create(m, n, LimV, LimH);
    }

    private int selectCell(ref Cell[,] celda0, int thisCell1, int thisCell2)
    {
        int escoger;
        bool value = true;
        int c = 0;

        while (value)
        {
            escoger = CurrentCell.Next(1, 5);

            switch (escoger)
            {
                case 1:
                    if (celda0[thisCell1, thisCell2].N == 1)
                        if (celda0[thisCell1 - 1, thisCell2].N != 0 &&
                            celda0[thisCell1 - 1, thisCell2].E != 0 &&
                            celda0[thisCell1 - 1, thisCell2].O != 0 &&
                            celda0[thisCell1 - 1, thisCell2].S != 0)
                        {
                            celda0[thisCell1, thisCell2].N = 0;
                            celda0[thisCell1 - 1, thisCell2].S = 0;
                            value = false;
                            c = 1;
                            //hará thisCelda1--;
                        }
                    break;

                case 2:
                    if (celda0[thisCell1, thisCell2].S == 1)
                        if (celda0[thisCell1 + 1, thisCell2].S != 0 &&
                            celda0[thisCell1 + 1, thisCell2].E != 0 &&
                            celda0[thisCell1 + 1, thisCell2].O != 0 &&
                            celda0[thisCell1 + 1, thisCell2].N != 0)
                        {
                            celda0[thisCell1, thisCell2].S = 0;
                            celda0[thisCell1 + 1, thisCell2].N = 0;
                            value = false;
                            c = 2;
                            //hará thisCelda1++;
                        }
                    break;

                case 3:
                    if (celda0[thisCell1, thisCell2].E == 1)
                        if (celda0[thisCell1, thisCell2 + 1].N != 0 &&
                            celda0[thisCell1, thisCell2 + 1].S != 0 &&
                            celda0[thisCell1, thisCell2 + 1].E != 0 &&
                            celda0[thisCell1, thisCell2 + 1].O != 0)
                        {
                            celda0[thisCell1, thisCell2].E = 0;
                            celda0[thisCell1, thisCell2 + 1].O = 0;
                            value = false;
                            c = 3;
                            //hará thisCelda2++;
                        }
                    break;

                case 4:
                    if (celda0[thisCell1, thisCell2].O == 1)
                        if (celda0[thisCell1, thisCell2 - 1].N != 0 &&
                            celda0[thisCell1, thisCell2 - 1].S != 0 &&
                            celda0[thisCell1, thisCell2 - 1].O != 0 &&
                            celda0[thisCell1, thisCell2 - 1].E != 0)
                        {
                            celda0[thisCell1, thisCell2].O = 0;
                            celda0[thisCell1, thisCell2 - 1].E = 0;
                            value = false;
                            c = 4;
                            // hará thisCelda2--;
                        }
                    break;
            }
        }

        return c;
    }

    public void Draw(int m, int n, int LimV, int LimH)
    {
        int m2 = 0, n2;
        GameObject cN = new GameObject("N");
        cN.transform.parent = Maze.transform;
        GameObject cS = new GameObject("S");
        cS.transform.parent = Maze.transform;
        GameObject cE = new GameObject("E");
        cE.transform.parent = Maze.transform;
        GameObject cO = new GameObject("O");
        cO.transform.parent = Maze.transform;
        for (int i = 0; i <= LimV; i += Padd)
        {
            n2 = 0;
            for (int j = 0; j <= LimH; j += Padd)
            {
                if (Cell0[m2, n2].N == 1 || Cell0[m2, n2].N == 3)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.parent = cN.transform;
                    cube.transform.localScale = new Vector3(Padd, 2, 0.05f);
                    cube.transform.Translate(new Vector3(j + 0.5f, 0, i));
                    cube.GetComponent<Renderer>().material.color = Color.red;
                    cube.AddComponent<BoxCollider>();
                }
                if (Cell0[m2, n2].S == 1 || Cell0[m2, n2].S == 3)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.parent = cS.transform;
                    cube.transform.localScale = new Vector3(Padd, 2, 0.05f);
                    cube.transform.Translate(new Vector3(j + 0.5f, 0, i + Padd));
                    cube.GetComponent<Renderer>().material.color = Color.red;
                    cube.AddComponent<BoxCollider>();
                }
                if (Cell0[m2, n2].O == 1 || Cell0[m2, n2].O == 3)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.parent = cO.transform;
                    cube.transform.localScale = new Vector3(0.05f, 2, Padd);
                    cube.transform.Translate(new Vector3(j, 0, i + Padd - 0.5f));
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    cube.AddComponent<BoxCollider>();
                }
                if (Cell0[m2, n2].E == 1 || Cell0[m2, n2].E == 3)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.parent = cE.transform;
                    cube.transform.localScale = new Vector3(0.05f, 2, Padd);
                    cube.transform.Translate(new Vector3(j + Padd, 0, Padd + i - 0.5f));
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    cube.AddComponent<BoxCollider>();
                }
                n2++;
                if (n2 == n) break;
            }
            m2++;
            if (m2 == m) break;
        }
        cN.transform.Translate(new Vector3(-Size / 2, 1, -Size / 2));
        cS.transform.Translate(new Vector3(-Size / 2, 1, -Size / 2));
        cE.transform.Translate(new Vector3(-Size / 2, 1, -Size / 2));
        cO.transform.Translate(new Vector3(-Size / 2, 1, -Size / 2));
    }

    public GameObject Maze;
    public int Size;
    void Start()
    {
        Size--;
        Padd = 1;
        CurrentCell = new Random(DateTime.Now.Millisecond);
        if (Size > 0)
        {
            (int m, int n, int LimiteV, int LimiteH) = Generate(Size, Size, true);
            Draw(m, n, LimiteV, LimiteH);
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.parent = Maze.transform;
            plane.transform.localScale = new Vector3(Size * 0.25f, 1, Size * 0.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
