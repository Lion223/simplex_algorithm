using System;
using System.Collections.Generic;

namespace SearchMethods
{
    public static class Simplex
    {
        // simplex table without basis variables
        private static double[,] table;

        // array indexes
        private static int m, n;

        // list of basis variables
        private static List<int> basis;

        // simplex table initiation
        private static void InitMatrix(double[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new double[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < n)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }

                // add coefficient 1 before basis variable in a row
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }

            n = table.GetLength(1);
        }

        // get new table results
        private static double[,] Calculate(double[] result)
        {
            int mainCol, mainRow; //indexes of the selected column and row

            while (!isEnd())
            {
                mainCol = isMainCol();
                mainRow = isMainRow(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }

            // result receives a value from x variable
            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);

                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }

        // check for completion (all values of the array are positive)
        private static bool isEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        // finding the master column
        private static int isMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        // finding the master row
        private static int isMainRow(int mainCol)
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;

            return mainRow;
        }

        public static void FindOpt(double[,] table)
        {
            double[] result = new double[2];
            double[,] table_result;
            InitMatrix(table);
            table_result = Calculate(result);

            Console.WriteLine("Final simplex matrix:");
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    Console.Write(table_result[i, j] + " ");
                Console.WriteLine();
            }

            Console.WriteLine($"{Environment.NewLine}Result:");
            Console.WriteLine("x[1] = " + result[0]);
            Console.WriteLine("x[2] = " + result[1]);
        }
    }
}
