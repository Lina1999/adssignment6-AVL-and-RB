using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBandAVL
{
    /// <summary>
    /// Color of node, which can be red or black.
    /// </summary>
    enum Color
    {
        red,
        black
    };

    /// <summary>
    /// generic class Red-black tree
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    class RBTree<TKey, TValue>: IDictionary<TKey, TValue> 
                   where TKey : IComparable<TKey>

    {
        /// <summary>
        /// class of RB tree node.
        /// </summary>
        private class RBNode
        {
            public Color color;
            public TKey key;
            public TValue value;
            public RBNode left;
            public RBNode right;
            public RBNode parent;
        }



        /// <summary>
        /// Class of RB leaf.
        /// </summary>
        private class RBLeaf
        {
           
            private static RBNode node;
            private RBLeaf() { }
            public static RBNode Instance()
            {
                if (node == null)
                {
                    node = new RBNode();
                    node.color = Color.black;
                }
                return node;
            }
        }
        /// <summary>
        /// RB root field.
        /// </summary>
        private RBNode RBroot;

        /// <summary>
        /// Dictionary entries.
        /// </summary>
        private List<RBNode> entries;

       /// <summary>
       /// parameterless constructor
       /// </summary>
        public RBTree()
        {
            RBroot = RBLeaf.Instance();
            entries = new List<RBNode>(count);
        }
        /// <summary>
        /// amount of nodes
        /// </summary>
        private int count;
        
        /// <summary>
        /// Getting keys of tree.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> ans=new List<TKey>(count);
                for (int i = 0; i < ans.Count; ++i)
                    ans[i] = entries[i].key;
                return ans;
            }
        }

        /// <summary>
        /// Getting values of tree.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> ans = new List<TValue>(count);
                for (int i = 0; i < ans.Count; ++i)
                    ans[i] = entries[i].value;
                return ans;
            }
        }

        /// <summary>
        /// Getting count.
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// bool func defining whether the object is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Indexer which is getting/setting key and returning its value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                return SearchKey(key).value;
            }
            set
            {
                SearchKey(key).value = value;
            }
        }

       /// <summary>
       /// Rotating left.
       /// </summary>
       /// <param name="node"></param>
       /// <returns></returns>
        private RBNode RBLeftRotate(RBNode node)
        {
            RBNode newNode = node.right;
            node.right = newNode.left;
            if (newNode.left != null)
                newNode.left.parent = node;
            newNode.parent = node.parent;
            if (node.parent == null)
                RBroot = newNode;
            else if (node == node.parent.left)
                node.parent.left = newNode;
            else
                node.parent.right = newNode;
            newNode.left = node;
            node.parent = newNode;
            return node;
        }

        /// <summary>
        /// rotating right
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private RBNode RBRightRotate(RBNode node)
        {
            RBNode newNode = node.left;
            node.left = newNode.right;
            if (newNode.right != null)
                newNode.right.parent = node;
            newNode.parent = node.parent;
            if (node.parent == null)
                RBroot = newNode;
            else if (node == node.parent.right)
                node.parent.right = newNode;
            else
                node.parent.left = newNode;
            newNode.right = node;
            node.parent = newNode;
            return node;
        }

        /// <summary>
        /// adding key and value
        /// </summary>
        /// <param name="elemKey"></param>
        /// <param name="elemVal"></param>
        public void Add(TKey elemKey, TValue elemVal)
        {

            RBNode newNode = new RBNode();
            newNode.key = elemKey;
            newNode.value = elemVal;
            newNode.left = RBLeaf.Instance();
            newNode.right = RBLeaf.Instance();
            Add(newNode);
            count++;
            entries.Add(newNode);
        }

        /// <summary>
        /// adding node
        /// </summary>
        /// <param name="elem"></param>
        private void Add(RBNode elem)
        {
            RBNode temp1 = RBLeaf.Instance();
            RBNode temp2 = RBroot;
            while (temp2 != RBLeaf.Instance())
            {
                temp1 = temp2;
                if (elem.key.CompareTo(temp2.key) < 0)
                    temp2 = temp2.left;
                else if (elem.key.CompareTo(temp2.key) > 0)
                    temp2 = temp2.right;
                else if (elem.key.CompareTo(temp2.key) == 0)
                    return;
            }

            elem.parent = temp1;
            if (temp1 == RBLeaf.Instance())
                RBroot = elem;
            else if (elem.key.CompareTo(temp1.key) < 0)
                temp1.left = elem;
            else if (elem.key.CompareTo(temp1.key) > 0)
                temp1.right = elem;
            else if (elem.key.CompareTo(temp1.key) == 0)
                return;
            elem.left = RBLeaf.Instance();
            elem.right = RBLeaf.Instance();
            elem.color = Color.red;
            elem = RBFixInsertion(elem);
        }

        /// <summary>
        /// fixing violations
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private RBNode RBFixInsertion(RBNode elem)
        {
            RBNode temp = new RBNode();

            while (elem.parent.color == Color.red)
            {
                if (elem.parent == elem.parent.parent.left)
                {
                    temp = elem.parent.parent.right;
                    if (temp.color == Color.red)
                    {
                        elem.parent.color = Color.black;
                        temp.color = Color.black;
                        elem.parent.parent.color = Color.red;
                        elem = elem.parent.parent;
                    }
                    else
                    {
                        if (elem == elem.parent.right)
                        {
                            elem = elem.parent;
                            RBLeftRotate(elem);
                        }
                        elem.parent.color = Color.black;
                        elem.parent.parent.color = Color.black;
                        RBRightRotate(elem.parent.parent);
                    }
                }
                else
                {
                    temp = elem.parent.parent.left;
                    if (temp.color == Color.red)
                    {
                        elem.parent.color = Color.black;
                        temp.color = Color.black;
                        elem.parent.parent.color = Color.red;
                        elem = elem.parent.parent;
                    }
                    else
                    {
                        if (elem == elem.parent.left)
                        {
                            elem = elem.parent;
                            RBRightRotate(elem);
                        }
                        elem.parent.color = Color.black;
                        elem.parent.parent.color = Color.red;
                        RBLeftRotate(elem.parent.parent);
                    }
                }
            }
            RBroot.color = Color.black;
            return elem;
        }

        /// <summary>
        /// Moving two nodes
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        private void RBMove(RBNode v1, RBNode v2)
        {
            if (v1.parent == RBLeaf.Instance())
                RBroot = v2;
            else if (v1 == v1.parent.left)
                v1.parent.left = v2;
            else
                v1.parent.right = v2;
            v2.parent = v1.parent;
        }

        /// <summary>
        /// deleting elem with its key
        /// </summary>
        /// <param name="elem"></param>
        public void RBDelete(TKey elem)
        {
            RBNode node = SearchKey(elem);
            RBDelete(node);
            count--;
            entries.Remove(node);
        }

        /// <summary>
        /// searching key in tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private RBNode SearchKey(TKey elem)
        {
            return SearchValue(RBroot, elem);
        }

        /// <summary>
        /// searching node in tree.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elemKey"></param>
        /// <returns></returns>
        private RBNode SearchValue(RBNode node, TKey elemKey)
        {
            if (node == null || elemKey.CompareTo(node.key) == 0)
                return node;
            if (elemKey.CompareTo(node.key) < 0)
                return SearchValue(node.left, elemKey);
            else
                return SearchValue(node.right, elemKey);
        }

        /// <summary>
        /// searching minimum element in tree.
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private RBNode RBMin(RBNode elem)
        {
            while (elem.left != RBLeaf.Instance())
                elem = elem.right;
            return elem;
        }

        /// <summary>
        /// deleting node
        /// </summary>
        /// <param name="elem"></param>
        private void RBDelete(RBNode elem)
        {
            RBNode temp1;
            RBNode temp2 = elem;
            Color tempColor = temp2.color;
            if (elem.left == RBLeaf.Instance())
            {
                temp1 = elem.right;
                RBMove(elem, elem.right);
            }

            else if (elem.right == RBLeaf.Instance())
            {
                temp1 = elem.left;
                RBMove(elem, elem.left);
            }
            else
            {
                temp2 = RBMin(elem.right);
                tempColor = temp2.color;
                temp1 = temp2.right;
                if (temp2.parent == elem)
                {
                    temp1.parent = temp2;
                }
                else
                {
                    RBMove(temp1, temp2.right);
                    temp2.right = elem.right;
                    temp2.right.parent = temp2;
                }
                RBMove(elem, temp2);
                temp2.left = elem.left;
                temp2.left.parent = temp2;
                temp2.color = elem.color;
            }
            if (tempColor == Color.black)
                RBFixDeletion(temp1);
        }

        /// <summary>
        /// fixing deletion violations
        /// </summary>
        /// <param name="elem"></param>
        private void RBFixDeletion(RBNode elem)
        {
            RBNode temp;
            while (elem != RBroot && elem.color == Color.black)
            {
                if (elem == elem.parent.left)
                {
                    temp = elem.parent.right;
                    if(temp!=null)
                    { 
                        if (temp.color == Color.red)
                        {
                            temp.color = Color.black;
                            elem.parent.color = Color.red;
                            RBLeftRotate(elem.parent);
                            temp = elem.parent.right;
                        }

                        if (temp.left != null && temp.right != null)
                        {
                            if (temp.left.color == Color.black && temp.right.color == Color.black)
                            {
                                temp.color = Color.red;
                                elem = elem.parent;
                            }

                            else
                            {
                                if (temp.right != null && temp.left != null)
                                {
                                    if (temp.right.color == Color.black)
                                    {
                                        temp.left.color = Color.black;
                                        temp.color = Color.red;
                                        RBRightRotate(temp);
                                        temp = elem.parent.right;
                                    }
                                    if (temp.right != null)
                                    {
                                        temp.color = elem.parent.color;
                                        elem.parent.color = Color.black;
                                        temp.right.color = Color.black;
                                        RBLeftRotate(elem.parent);
                                        elem = RBroot;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    temp = elem.parent.left;
                    if (temp != null)
                    {
                        if (temp.color == Color.red)
                        {
                            temp.color = Color.black;
                            elem.parent.color = Color.red;
                            RBRightRotate(elem.parent);
                            temp = elem.parent.left;
                        }
                        if (temp.left != null && temp.right != null)
                        {
                            if (temp.left.color == Color.black && temp.right.color == Color.black)
                            {
                                temp.color = Color.red;
                                elem = elem.parent;
                            }
                            else
                            {
                                if (temp.left != null && temp.right != null)
                                {
                                    if (temp.left.color == Color.black)
                                    {
                                        temp.right.color = Color.black;
                                        temp.color = Color.red;
                                        RBLeftRotate(temp);
                                        temp = elem.parent.left;
                                    }
                                    if (temp.left != null && temp.right != null)
                                    {
                                        temp.color = elem.parent.color;
                                        elem.parent.color = Color.black;
                                        temp.right.color = Color.black;
                                        RBRightRotate(elem.parent);
                                        elem = RBroot;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            elem.color = Color.black;
        }

 
        /// <summary>
        /// Getting node with key
        /// </summary>
        /// <param name="elemKey"></param>
        /// <returns></returns>
        private RBNode GetNode(TKey elemKey)
        {
           RBNode node = RBroot;
            while (node!=RBLeaf.Instance())
            {
                if (elemKey.CompareTo(node.key) < 0)
                    node = node.left;
                else if (elemKey.CompareTo(node.key) > 0)
                    node = node.right;
                else
                    return node;
            }
            return null;
        }

        /// <summary>
        /// Finding out if tree contains the key
        /// </summary>
        /// <param name="elemKey"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey elemKey)
        {
           return (GetNode(elemKey)!=null);
        }

        /// <summary>
        /// removing elem with key
        /// </summary>
        /// <param name="elemKey"></param>
        /// <returns></returns>
        public bool Remove(TKey elemKey)
        {
            if (!ContainsKey(elemKey))
                return false;
            RBDelete(elemKey);
            return true;
        }

        /// <summary>
        /// using contains key changing elemValue with elemkey's value
        /// </summary>
        /// <param name="elemKey"></param>
        /// <param name="elemValue"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey elemKey, out TValue elemValue)
        {
            if (ContainsKey(elemKey))
            {
                for (int i = 0; i < entries.Count; ++i)
                {
                    if (entries[i].key.Equals(elemKey))
                    {
                        elemValue = entries[i].value;
                        return true;
                    }
                }
            }
            elemValue = default(TValue);
            return false;
        }

        /// <summary>
        /// adding KeyValue pair
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// clearing tree
        /// </summary>
        public void Clear()
        {
            entries.Clear();
            RBroot = null;
            count = 0;
        }

        /// <summary>
        /// Finding out if tree contains the pair
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (ContainsKey(item.Key) && (this[item.Key]).Equals(item.Value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// copying to index from tree.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if(array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("Out of range");
            if (array.Length - arrayIndex < entries.Count)
                throw new ArgumentException("Wrong output.");
            for (int i = 0; i < entries.Count; ++i)
                array[i + arrayIndex] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
        }
        
        /// <summary>
        /// removing item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (count == 0 || !ContainsKey(item.Key))
                return false;
            RBDelete(item.Key);
            return true;
        }

        /// <summary>
        ///getting enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (RBroot != RBLeaf.Instance())
            {
                RBNode elem = RBroot;
                Stack<RBNode> nodes = new Stack<RBNode>();
                bool elemsOnLeft = true;
                nodes.Push(elem);
                while (nodes.Count > 0)
                {
                    if (elemsOnLeft)
                    {
                        while (elem.left != RBLeaf.Instance())
                        {
                            nodes.Push(elem);
                            elem = elem.left;
                        }
                    }
                   yield return new KeyValuePair<TKey, TValue>(elem.key, elem.value);

                   if (elem.right == RBLeaf.Instance())
                   {
                       elem = nodes.Pop();
                       elemsOnLeft = false;
                   }
                   else
                   {
                       elem = elem.right;
                       elemsOnLeft = true;
                   }
                   
                }
            }
            
        }

        /// <summary>
        /// getting enumerator 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

  
