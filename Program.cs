using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBandAVL
{
    
    class Program
    {
        /// <summary>
        /// getting avl insertion time
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        static TimeSpan getInsertionTime(ref AVLTree<int, string> dict, int entries)
        {
            Random r1 = new Random();
            Random r2 = new Random();
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < entries; ++i)
            {
                dict.Add(r1.Next(), r2.Next().ToString());
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// getting avl searching time
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        static TimeSpan getSearchingTime(IDictionary<int, string> dict)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var i in dict)
            {
                dict.Contains(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// getting avl deletion time
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        static TimeSpan getRemovalTime(ref AVLTree<int, string> dict)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var i in dict)
            {
                dict.Remove(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// getting hash tabled dictionary insertion time
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        static TimeSpan getInsertionTime(ref Dictionary<int, string> dict, int entries)
        {
            Random r1 = new Random();
            Random r2 = new Random();
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < entries; ++i)
            {
                dict.Add(r1.Next(), r2.Next().ToString());
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// getting rb insertion time
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        static TimeSpan getInsertionTime(ref RBTree<int, string> dict, int entries)
        {
            Random r1 = new Random();
            Random r2 = new Random();
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < entries; ++i)
            {
                dict.Add(r1.Next(), r2.Next().ToString());
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// getting rb deletion time
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        static TimeSpan getRemovalTime(ref RBTree<int, string> dict)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var i in dict)
            {
                dict.Remove(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        static void Main(string[] args)
        {
            AVLTree<int, string> AVLtree = new AVLTree<int, string>();
            RBTree<int, string> RBtree = new RBTree<int, string>();
            Dictionary<int, string> dict1 = new Dictionary<int, string>();
            Dictionary<int, string> dict2 = new Dictionary<int, string>();
            Dictionary<int, string> dict3 = new Dictionary<int, string>();

            Console.WriteLine("AVL inserting time with 320 entries is " + getInsertionTime(ref AVLtree, 320));
            Console.WriteLine("AVL searching time with 320 entries is " + getSearchingTime(AVLtree));
            Console.WriteLine("AVL removal time with 320 entries is " + getRemovalTime(ref AVLtree));
            Console.WriteLine("AVL inserting time with 640 entries is "+ getInsertionTime(ref AVLtree, 640));
            Console.WriteLine("AVL searching time with 640 entries is " + getSearchingTime(AVLtree));
            Console.WriteLine("AVL removal time with 640 entries is " + getRemovalTime(ref AVLtree));
            Console.WriteLine("AVL inserting time with 1280 entries is " + getInsertionTime(ref AVLtree, 1280));
            Console.WriteLine("AVL searching time with 1280 entries is " + getSearchingTime(AVLtree));
            Console.WriteLine("AVL removal time with 1280 entries is " + getRemovalTime(ref AVLtree));

            Console.WriteLine("Dictionary inserting time with 320 entries is " + getInsertionTime(ref dict1, 320));
            Console.WriteLine("Dictionary searching time with 320 entries is " + getSearchingTime(dict1));
            Console.WriteLine("Dictionary inserting time with 640 entries is " + getInsertionTime(ref dict2, 640));
            Console.WriteLine("Dictionary searching time with 640 entries is " + getSearchingTime(dict2));
            Console.WriteLine("Dictionary inserting time with 1280 entries is " + getInsertionTime(ref dict3, 1280));
            Console.WriteLine("Dictionary searching time with 1280 entries is " + getSearchingTime(dict3));


            Console.WriteLine("RB inserting time with 320 entries is " + getInsertionTime(ref RBtree, 320));
            Console.WriteLine("RB searching time with 320 entries is " + getSearchingTime(RBtree));
            //Console.WriteLine("RB removal time with 320 entries is " + getRemovalTime(ref RBtree));
            Console.WriteLine("RB inserting time with 640 entries is " + getInsertionTime(ref RBtree, 640));
            Console.WriteLine("RB searching time with 640 entries is " + getSearchingTime(RBtree));
           // Console.WriteLine("RB removal time with 640 entries is " + getRemovalTime(ref RBtree));
            Console.WriteLine("RB inserting time with 1280 entries is " + getInsertionTime(ref RBtree, 1280));
           Console.WriteLine("RB searching time with 1280 entries is " + getSearchingTime(RBtree));
           // Console.WriteLine("RB removal time with 1280 entries is " + getRemovalTime(ref RBtree));
        }
    }

    
}
