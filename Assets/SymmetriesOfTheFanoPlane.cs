using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rnd = UnityEngine.Random;
using KModkit;

public class SymmetriesOfTheFanoPlane : MonoBehaviour {
    public KMSelectable[] buttons;
    public MeshFilter cube;
    public SpriteRenderer[] moduleMatricesFlat;
    public Sprite[] matrixSprites;
    public SymmetriesOfTheFanoPlane sfp;
    SpriteRenderer[][,] moduleMatrices = new SpriteRenderer[3][,] { new SpriteRenderer[3, 3], new SpriteRenderer[3, 3], new SpriteRenderer[3, 3] };
    public int[][,] firstMatrices = new int[3][,] { new int[3, 3], new int[3, 3], new int[3, 3] };
    int[][,] secondMatrices = new int[3][,] { new int[3, 3], new int[3, 3], new int[3, 3] };
    public int[][,] productMatrices = new int[3][,] { new int[3, 3], new int[3, 3], new int[3, 3] };
    public int[][,] andMatrices = new int[3][,] { new int[3, 3], new int[3, 3], new int[3, 3] };
    public int[][,] impliesMatrices = new int[3][,] { new int[3, 3], new int[3, 3], new int[3, 3] };
    public SFPCoroutineHelper coroutineHelper;
    public KMBombModule module;
    public KMAudio sound;
    int[][] points = new int[][] { new int[] { 1, 1, 1 }, new int[] { 1, 1, 0 }, new int[] { 0, 1, 1 }, new int[] { 1, 0, 1 }, new int[] { 1, 0, 0 }, new int[] { 0, 1, 0 }, new int[] { 0, 0, 1 } };
    int[][] planeState = new int[7][];
    int moduleId;
    static int moduleIdCounter = 1;
    bool solved;
    // Use this for initialization
    void Awake() {
        moduleId = moduleIdCounter++;
        //for (int i = 0; i < 7; i++) { int j = i; buttons[j].OnInteract += delegate { PressButton(j); return false; }; }
        module.OnActivate += delegate { Activate(); };
    }

    void Activate()
    {
        SeedMatrices(firstMatrices);
        SeedMatrices(secondMatrices);
        SetUpHelperMatrices();
        coroutineHelper.Begin();
    }

    // Update is called once per frame
    void SeedMatrices(int[][,] array) {
        foreach (int[,] matrix in array)
        {
            do
            {
                for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++)
                    {
                        matrix[i, j] = rnd.Range(0, 2);
                    }
            } while (SFPFunctionsHelper.Determinant(matrix) == 0);
        }
    }

    void SetUpHelperMatrices()
    {
        for (int i = 0; i < 3; i++)
        {
            productMatrices[i] = SFPFunctionsHelper.MatrixProduct(firstMatrices[i], secondMatrices[i]);
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    andMatrices[i][j, k] = firstMatrices[i][j, k] * secondMatrices[i][j, k];
                    impliesMatrices[i][j, k] = SFPFunctionsHelper.Implies(firstMatrices[i][j, k], secondMatrices[i][j, k]);
                }
            }
        }
    }
    void PermutePlane()
    {
        points.CopyTo(planeState, 0);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                planeState[i] = SFPFunctionsHelper.VectorMatrixProduct(planeState[j], firstMatrices[i]);
            }
        }
    }
    void PressButton(int button)
    {

    }

    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        string validcmds = "1234567";
        if (command.Contains(' '))
        {

            yield return "sendtochaterror @{0}, invalid command.";

            yield break;

        }
        else
        {
            for (int i = 0; i < command.Length; i++)
            {
                if (!validcmds.Contains(command[i]))
                {
                    yield return "sendtochaterror Invalid command.";
                    yield break;
                }
            }
            for (int i = 0; i < command.Length; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (command[i] == validcmds[j])
                    {
                        yield return new WaitForSeconds(0.1f);
                        buttons[j].OnInteract();
                    }
                }
            }
        }
    }

}
