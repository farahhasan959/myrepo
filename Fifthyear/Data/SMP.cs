/*
 * usage:
 *  SMP smp = new SMP();
 *  
 *  
 *  solve method takes 3 arguments:
            1- number of men or women
            2- ordered list for men, every man orders all women from best to worst
            3- ordered list for women, evert woman orders all men from best to worst
 
    returns list of KeyValuePair {man, woman), the marhting
 *  
 *  List<KeyValuePair<int , int>> answer = smp.solve(n, men, women);
 *  then print the answer
 */


using System.Collections.Generic;

class SMP
{
    int n;
    Queue<int> freeMen = new Queue<int>();
    int[] pointer;
    int[] chosenMan;
    int[,] likeness;
    int[,] menLists;

    void init(int n, int[,] men, int[,] women)
    {
        this.n = n;


        // first all men are free(no one is engaged yet)
        for (int i = 0; i < n; i++)
        {
            freeMen.Enqueue(i);
        }

        // all men are pointing to the first woman in their lists, so pointer[i] = 0; for all men
        pointer = new int[n];

        // first no woman has chosen any man
        chosenMan = new int[n];
        for (int i = 0; i < n; i++)
        {
            chosenMan[i] = -1;
        }

        // lists of men
        menLists = men;

        // how much does a woman like a man, the smaller the better
        likeness = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                likeness[i, women[i, j]] = j;
            }
        }
    }

    void process()
    {
        while (freeMen.Count > 0)
        {
            int m = freeMen.Dequeue();
            int w = menLists[m, pointer[m]];

            if (chosenMan[w] != -1)
            {
                if (likeness[w, m] < likeness[w, chosenMan[w]])
                {
                    freeMen.Enqueue(chosenMan[w]);
                    pointer[chosenMan[w]]++;
                    chosenMan[w] = m;
                }
                else
                {
                    freeMen.Enqueue(m);
                    pointer[m]++;
                }
            }
            else
            {
                chosenMan[w] = m;
            }
        }
    }

    public List<KeyValuePair<int, int>> solve(int n, int[,] men, int[,] women)
    {
        init(n, men, women);
        process();


        List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();

        for (int i = 0; i < n; i++)
        {
            list.Add(new KeyValuePair<int, int>(chosenMan[i], i));
        }


        return list;
    }
}