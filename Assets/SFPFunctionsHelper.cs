using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFPFunctionsHelper {

	// Use this for initialization
	public static int Implies (int left, int right) {
		switch (left)
        {
            case 0:
                return 1;
            case 1:
                switch (right)
                {
                    case 0:
                        return 0;
                    case 1:
                        return 1;     
                }
                break;
        }
        return 2; //This should never happen and is only there to prevent an error.
	}
	
	
	public static int[] VectorMatrixProduct (int[] vec, int[,] mat) {
        var new_vec = new int[3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                new_vec[x] += vec[y] * mat[x, y];
            }
            //Necesary because we're working in GF(2)
            new_vec[x] = new_vec[x] % 2;
        }
        return new_vec;
    }

    public static Vector3 VectorMatrixProduct(Vector3 vec, int[,] mat)
    {
        var new_vec = new int[3];
        var old_vec = new int[3] {(int) vec.x, (int)vec.y, (int) vec.z};
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                new_vec[x] += old_vec[y] * mat[x, y];
            }
            //Necesary because we're working in GF(2)
            new_vec[x] = new_vec[x] % 2;
        }
        return new Vector3(new_vec[0], new_vec[1], new_vec[2]);
    }

    public static int[,] MatrixProduct(int[,] mat_left, int[,] mat_right)
    {
        var new_mat = new int[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    new_mat[x, y] += mat_left[x, z] * mat_right[z, y];
                }
                new_mat[x, y] = new_mat[x, y] % 2;
            }
            //Necesary because we're working in GF(2)
            
        }
        return new_mat;
    }
    public static int Determinant(int[,] matrix)
    {
        return( (matrix[0,0] * matrix [1,1] * matrix [2,2]) + (matrix [0,0] * matrix [2,1] * matrix [1,2]) + (matrix[1,0] * matrix[0,1] * matrix[2, 2]) + (matrix[2,0] * matrix [1,1] * matrix [0,2]) + (matrix [2,0] * matrix[1,2] * matrix [0,1]) + (matrix[0,2] * matrix[1,0] * matrix [2,1]) )% 2;
    }
}
