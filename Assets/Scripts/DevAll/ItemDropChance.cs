using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DevAll.Testing;

namespace DevAll.DropChance
{
    public class ItemDropChance : MonoBehaviour
    {
        TestSettings DevTest; 
        //since my items is below 127 I am using short to make it more efficient
        List<short> items = new List<short>();
        private void Awake()
        {
            DevTest = GameObject.FindObjectOfType<TestSettings>();
        }
        #region LoadTesting
        //void Start()
        //{
        //    //for testing purposes
        //    int arrayNum = 0;
        //    for (int i = 0; i < 10000; i++)
        //    {
        //        items.Add((short)Random.Range(0, 100));
        //    }
        //    //items.Add(1);
        //    //items.Add(1);
        //    //items.Add(1);
        //    //items.Add(2);
        //    //items.Add(3);
        //    //items.Add(4);
        //    //items.Add(4);

        //    print(items.Count);
        //    print(string.Format("Here's the list: ({0}).", string.Join(", ", items)));
        //    IListExtensions.Shuffle(items);
        //    print(string.Format("Here's the list: ({0}).", string.Join(", ", items)));
        //    arrayNum = Random.Range(0, items.Count);
        //    print(arrayNum + " || " + items[arrayNum]);
        //}
        #endregion

        // drop chance 1 is normal, 2 twice likely it will drop, 3 times likely....
        // need the itemID
        public void AddItems(short itemID, short dropChance)
        {
            for (int i = 0; i < dropChance; i++)
            {
                items.Add(itemID);
            }
        }

        // You will need to call shuffle manually through your code if you want it shuffled
        // before calling get items. Recomended use at least once.
        public void ShuffleItems()
        {
                if (DevTest.chk_DropChance) { print(string.Format("Before shuffling list: ({0}).", string.Join(", ", items))); } 
            IListExtensions.Shuffle(items);
                if (DevTest.chk_DropChance) { print(string.Format("After shuffling list: ({0}).", string.Join(", ", items))); }
        }

        //Get item that is randomely called.
        public short GetItem()
        {
            int RandomedItem = Random.Range(0, items.Count);
                if (DevTest.chk_DropChance) { print("IDC randomed element: " + RandomedItem); }
            return items[RandomedItem];
        }
    }

    public static class IListExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}

