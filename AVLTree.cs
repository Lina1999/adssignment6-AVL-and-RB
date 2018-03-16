using System;
using System.Collections;
using System.Collections.Generic;

namespace RBandAVL
{
    /// <summary>
    /// class AVL tree
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class AVLTree<TKey, TValue>: IDictionary<TKey, TValue>
                           where TKey : IComparable<TKey>
    {
        /// <summary>
        /// list of entries
        /// </summary>
        private List<AVLNode> entries;

        /// <summary>
        /// entries count
        /// </summary>
        private int count;

        /// <summary>
        /// root of the tree
        /// </summary>
        private AVLNode AVLroot;

        /// <summary>
        /// parameterless constructor
        /// </summary>
        public AVLTree()
        {
            entries = new List<AVLNode>(count);
        }

        /// <summary>
        /// getting keys
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> ans = new List<TKey>(count);
                for (int i = 0; i < ans.Count; ++i)
                    ans[i] = entries[i].Key;
                return ans;
            }
        }

        /// <summary>
        /// getting values
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> ans = new List<TValue>(count);
                for (int i = 0; i < ans.Count; ++i)
                    ans[i] = entries[i].Value;
                return ans;
            }
        }

        /// <summary>
        /// getting count
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// finding out if the obj is read only
        /// </summary>
       public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// indexer getting ey and returning its value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue ans;
                Search(key, out ans);
                return ans;
            }
            set
            {
                this[key] = value;
            }
        }

        /// <summary>
        /// class of the AVL node
        /// </summary>
        private class AVLNode
        {
            public int balanceFactor;
            public AVLNode Parent;
            public AVLNode Right;
            public AVLNode Left;
            public TKey Key;
            public TValue Value;
            public AVLNode() { }
            public AVLNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
            public AVLNode(TKey key, TValue value, AVLNode parent)
            {
                Key = key;
                Value = value;
                Parent = parent;
            }
        }
       
        /// <summary>
        /// adding element
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            count++;
            entries.Add(new AVLNode(key, value));
            if (AVLroot == null)
                AVLroot = new AVLNode(key, value);
            else
            {
                AVLNode node = AVLroot;

                while (node != null)
                {
                    if (key.CompareTo(node.Key) < 0)
                    {
                        AVLNode left = node.Left;

                        if (left == null)
                        {
                            node.Left = new AVLNode(key, value, node);
                            Balancing(node, 1);
                            return;
                        }
                        else
                        {
                            node = left;
                        }
                    }
                    else if (key.CompareTo(node.Key) > 0)
                    {
                        AVLNode right = node.Right;

                        if (right == null)
                        {
                            node.Right = new AVLNode(key, value, node);
                            Balancing(node, -1);
                            return;
                        }
                        else
                        {
                            node = right;
                        }
                    }
                    else
                    {
                        node.Value = value;
                        return;
                    }
                }
            }
            
        }

        /// <summary>
        /// balancing the tree
        /// </summary>
        /// <param name="node"></param>
        /// <param name="balanceFactor"></param>
        private void Balancing(AVLNode node, int balanceFactor)
        {
            while (node != null)
            {
                node.balanceFactor += balanceFactor;
                balanceFactor = node.balanceFactor;

                if (balanceFactor == 0)
                {
                    return;
                }
                else if (balanceFactor == 2)
                {
                    if (node.Left.balanceFactor == 1)
                        RightRotation(node);
                    else
                        LeftRightRotation(node);
                    return;
                }
                else if (balanceFactor == -2)
                {
                    if (node.Right.balanceFactor == -1)
                        LeftRotation(node);
                    else
                        RightLeftRotation(node);
                    return;
                }

                AVLNode nodeP = node.Parent;
                if (nodeP != null)
                {
                    if (nodeP.Left == node)
                        balanceFactor = 1;
                    else
                        balanceFactor = -1;
                }

                node = nodeP;
            }
        }

        /// <summary>
        /// left rotate
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private AVLNode LeftRotation(AVLNode node)
        {
            AVLNode r = node.Right;
            AVLNode rl = r.Left;
            AVLNode parent = node.Parent;
            r.Parent = parent;
            r.Left = node;
            node.Right = rl;
            node.Parent = r;
            if (rl != null)
                rl.Parent = node;
            if (node == AVLroot)
                AVLroot = r;
            else if (parent.Right == node)
                 parent.Right = r;
            else
                 parent.Left = r;
            r.balanceFactor++;
            node.balanceFactor = -r.balanceFactor;
            return r;
        }

        /// <summary>
        /// right rotate
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private AVLNode RightRotation(AVLNode elem)
        {
            AVLNode l = elem.Left;
            AVLNode lr = l.Right;
            AVLNode parent = elem.Parent;
            l.Parent = parent;
            l.Right = elem;
            elem.Left = lr;
            elem.Parent = l;
            if (lr != null)
                lr.Parent = elem;
            if (elem == AVLroot)
                AVLroot = l;
            else if (parent.Left == elem)
                parent.Left = l;
            else
                parent.Right = l;
            --l.balanceFactor;
            elem.balanceFactor = -l.balanceFactor;
            return l;
        }


        /// <summary>
        /// left right rotate
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private AVLNode LeftRightRotation(AVLNode node)
        {
            AVLNode l = node.Left;
            AVLNode lr = l.Right;
            AVLNode nodeP = node.Parent;
            //AVLNode lrr = lr.Right;
            AVLNode lrr = (l.Right).Right;

            AVLNode lrl = lr.Left;
            lr.Parent = nodeP;
            node.Left = lrr;
            l.Right = lrl;
            lr.Left = l;
            lr.Right = node;
            l.Parent = lr;
            node.Parent = lr;

            if (lrr != null)

                lrr.Parent = node;
            

            if (lrl != null)
                lrl.Parent = l;
            if (node == AVLroot)
                AVLroot = lr;
            else if (nodeP.Left == node)
                nodeP.Left = lr;
            else
                nodeP.Right = lr;
            if (lr.balanceFactor == -1)
            {
                node.balanceFactor = 0;
                l.balanceFactor = 1;
            }
            else if (lr.balanceFactor == 0)
            {
                node.balanceFactor = 0;
                l.balanceFactor = 0;
            }
            else
            {
                node.balanceFactor = -1;
                l.balanceFactor = 0;
            }
            lr.balanceFactor = 0;
            return lr;
        }


        /// <summary>
        /// right left rotate
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private AVLNode RightLeftRotation(AVLNode node)
        {
            AVLNode r = node.Right;
            AVLNode rl = r.Left;
            AVLNode NodeP = node.Parent;
            AVLNode rll = rl.Left;
            AVLNode rlr = rl.Right;
            rl.Parent = NodeP;
            node.Right = rll;
            r.Left = rlr;
            rl.Right = r;
            rl.Left = node;
            r.Parent = rl;
            node.Parent = rl;
            if (rll != null)
                rll.Parent = node;
            if (rlr != null)
                rlr.Parent = r;
            if (node == AVLroot)
                AVLroot = rl;
            else if (NodeP.Right == node)
                NodeP.Right = rl;
            else
                NodeP.Left = rl;
            if (rl.balanceFactor == 1)
            {
                node.balanceFactor = 0;
                r.balanceFactor = -1;
            }
            else if (rl.balanceFactor == 0)
            {
                node.balanceFactor = 0;
                r.balanceFactor = 0;
            }
            else
            {
                node.balanceFactor = 1;
                r.balanceFactor = 0;
            }

            rl.balanceFactor = 0;
            return rl;
        }

        /// <summary>
        /// removing node with key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            AVLNode node = AVLroot;
            while (node != null)
            {
                if (key.CompareTo(node.Key) < 0)
                {
                    node = node.Left;
                }
                else if (key.CompareTo(node.Key) > 0)
                {
                    node = node.Right;
                }
                else
                {
                    AVLNode l = node.Left;
                    AVLNode r = node.Right;
                    if (l == null)
                    {
                        if (r == null)
                        {
                            if (node == AVLroot)
                            {
                                AVLroot = null;
                            }
                            else
                            {
                                AVLNode nodeP = node.Parent;
                                if (nodeP.Left == node)
                                {
                                    nodeP.Left = null;
                                    RemoveBalanceFactor(nodeP, -1);
                                }
                                else
                                {
                                    nodeP.Right = null;
                                    RemoveBalanceFactor(nodeP, 1);
                                }
                            }
                        }
                        else
                        {
                            Change(node, r);
                            RemoveBalanceFactor(node, 0);
                        }
                    }
                    else if (r == null)
                    {
                        Change(node, l);
                        RemoveBalanceFactor(node, 0);
                    }
                    else
                    {
                        AVLNode temp = r;
                        if (temp.Left == null)
                        {
                            AVLNode parent = node.Parent;

                            temp.Parent = parent;
                            temp.Left = l;
                            temp.balanceFactor = node.balanceFactor;

                            if (l != null)
                            {
                                l.Parent = temp;
                            }

                            if (node == AVLroot)
                            {
                                AVLroot = temp;
                            }
                            else
                            {
                                if (parent.Left == node)
                                {
                                    parent.Left = temp;
                                }
                                else
                                {
                                    parent.Right = temp;
                                }
                            }

                            RemoveBalanceFactor(temp, 1);
                        }
                        else
                        {
                            while (temp.Left != null)
                                temp = temp.Left;
                            AVLNode parent = node.Parent;
                            AVLNode tempParent = temp.Parent;
                            AVLNode tempRight = temp.Right;
                            if (tempParent.Left == temp)
                                tempParent.Left = tempRight;
                            else
                                tempParent.Right = tempRight;
                            if (tempRight != null)
                                tempRight.Parent = tempParent;
                            temp.Parent = parent;
                            temp.Left = l;
                            temp.balanceFactor = node.balanceFactor;
                            temp.Right = r;
                            r.Parent = temp;
                            if (l != null)
                                l.Parent = temp;
                            if (node == AVLroot)
                            {
                                AVLroot = temp;
                            }
                            else
                            {
                                if (parent.Left == node)
                                    parent.Left = temp;
                                else
                                    parent.Right = temp;
                            }
                            RemoveBalanceFactor(tempParent, -1);
                        }
                    }
                    count--;
                    entries.Remove(new AVLNode(key, this[key]));
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// removing balance
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="balanceFactor"></param>
        private void RemoveBalanceFactor(AVLNode elem, int balanceFactor)
        {
            while (elem != null)
            {
                elem.balanceFactor += balanceFactor;
                balanceFactor = elem.balanceFactor;
                if (balanceFactor == 2)
                {
                    if (elem.Left.balanceFactor >= 0)
                    {
                        elem = RightRotation(elem);
                        if (elem.balanceFactor == -1)
                            return;
                    }
                    else
                    {
                        elem = LeftRightRotation(elem);
                    }
                }
                else if (balanceFactor == -2)
                {
                    if (elem.Right.balanceFactor <= 0)
                    {
                        elem = LeftRotation(elem);
                        if (elem.balanceFactor == 1)
                            return;
                    }
                    else
                    {
                        elem = RightLeftRotation(elem);
                    }
                }
                else if (balanceFactor != 0)
                {
                    return;
                }

                AVLNode p = elem.Parent;
                if (p != null)
                {
                    if (p.Left == elem)
                        balanceFactor = -1;
                    else
                        balanceFactor = 1;
                }
                elem = p;
            }
        }
        
        /// <summary>
        /// changing node
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        private void Change(AVLNode to, AVLNode from)
        {
            AVLNode lNode = from.Left;
            AVLNode rNode = from.Right;
            to.balanceFactor = from.balanceFactor;
            to.Key = from.Key;
            to.Value = from.Value;
            to.Left = lNode;
            to.Right = rNode;
            if (rNode != null)
                rNode.Parent = to;
            if (lNode != null)
                lNode.Parent = to;
        }

        /// <summary>
        /// searching key and writing its value in elemValue
        /// </summary>
        /// <param name="elemKey"></param>
        /// <param name="elemValue"></param>
        /// <returns></returns>
        public bool Search(TKey elemKey, out TValue elemValue)
        {
            AVLNode elem = AVLroot;
            while (elem != null)
            {
                if (elemKey.CompareTo(elem.Key) < 0)
                {
                    elem = elem.Left;
                }
                else if (elemKey.CompareTo(elem.Key) > 0)
                {
                    elem = elem.Right;
                }
                else
                {
                    elemValue = elem.Value;
                    return true;
                }
            }
            elemValue = default(TValue);
            return false;
        }

        /// <summary>
        /// Finding out if the tree conatains the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            TValue temp;
            return Search(key, out temp);
        }

        /// <summary>
        /// Finding out if the tree conatains the key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (ContainsKey(key))
            {
                for (int i = 0; i < entries.Count; ++i)
                {
                    if (entries[i].Key.Equals(key))
                    {
                        value = entries[i].Value;
                        return true;
                    }
                }
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// adding item
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// clearing the tree
        /// </summary>
        public void Clear()
        {
            entries.Clear();
            AVLroot = null;
            count = 0;
        }

        /// <summary>
        /// finding out if the tree contains the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if(ContainsKey(item.Key) && (this[item.Key]).Equals(item.Value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// copying to array from tree
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("Out of range");
            if (array.Length - arrayIndex < entries.Count)
                throw new ArgumentException("Wrong output.");
            for (int i = 0; i < entries.Count; ++i)
                array[i + arrayIndex] = new KeyValuePair<TKey, TValue>(entries[i].Key, entries[i].Value);
        }

        /// <summary>
        /// removing item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Remove(item.Key))
                return true;
            return false;
        }

        /// <summary>
        /// enumerating
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (AVLroot != null)
            {
                AVLNode elem = AVLroot;
                Stack<AVLNode> nodes = new Stack<AVLNode>();
                bool elemsOnLeft = true;
                nodes.Push(elem);
                while (nodes.Count > 0)
                {
                    if (elemsOnLeft)
                    {
                        while (elem.Left != null)
                        {
                            nodes.Push(elem);
                            elem = elem.Left;
                        }
                    }

                    yield return new KeyValuePair<TKey, TValue>(elem.Key, elem.Value);
                    if (elem.Right == null)
                    {
                        elem = nodes.Pop();
                        elemsOnLeft = false;
                    }
                    else
                    {
                        elem = elem.Right;
                        elemsOnLeft = true;
                    }
                }
            }
        }

        /// <summary>
        /// enumerating
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}