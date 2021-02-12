using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFPCoroutineHelper: MonoBehaviour {
    public MeshFilter cube;
    public SpriteRenderer[] moduleMatricesFlat;
    public Sprite[] matrixSprites;
    public SymmetriesOfTheFanoPlane sfp;
    SpriteRenderer[][,] moduleMatrices = new SpriteRenderer[3][,] { new SpriteRenderer[3, 3], new SpriteRenderer[3, 3], new SpriteRenderer[3, 3] };
    // Use this for initialization
    public void Begin () {
        for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) for (int k = 0; k < 3; k++) moduleMatrices[i][j, k] = moduleMatricesFlat[9 * i + 3 * j + k];
        RenderMatrix(0, sfp.productMatrices[0]);
        RenderMatrix(1, sfp.andMatrices[0]);
        RenderMatrix(2, sfp.impliesMatrices[0]);
        StartCoroutine(Controller());
    }

    // Update is called once per frame
    void RenderMatrix(int index, int[,] matrix) {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                moduleMatrices[index][i, j].sprite = matrixSprites[matrix[i, j]];
            }
        }
    }
    IEnumerator Controller()
    {
        while (true)
        {
            int count = 0;
            while (count < 3)
            {
                StartCoroutine(ShuffleCube(count));
                yield return new WaitForSeconds(3f);
                count++;
            }
        }
    }
    IEnumerator ShuffleCube(int index)
    {
        Vector3[] newVertices = new Vector3[24];
        cube.mesh.vertices.CopyTo(newVertices, 0);
        int count = 0;
        while (count < 10)
        {
            for (int i = 0; i < 24; i++)
            {
                newVertices[i] = Vector3.Lerp(newVertices[i], SFPFunctionsHelper.VectorMatrixProduct(newVertices[i], sfp.productMatrices[index]), 0.1f);
            }
            cube.mesh.vertices = newVertices;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

