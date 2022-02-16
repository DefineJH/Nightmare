﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BT
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject
    {
        public Node RootNode;
        public Node.State TreeState = Node.State.InProgress;
        //현재 BT서 사용되는 모든 노드
        public List<Node> nodes = new List<Node>();
        void Traverse(BT.Node node,System.Action<Node> visitor)
        {
            if(node)
            {
                visitor.Invoke(node);
                var childs = GetChilds(node);
                childs.ForEach((n) => Traverse(n, visitor));
            }
        }

        public BehaviorTree Clone()
        {
            BehaviorTree bTree = Instantiate(this);
            bTree.RootNode = bTree.RootNode.Clone();
            bTree.nodes = new List<Node>();
            Traverse(bTree.RootNode, (n) =>
             {
                 bTree.nodes.Add(n);
             });
            return bTree;
        }
        public Node.State Update()
        {
            //rootNode 가 InProgress가 아닐 경우, Update 진행X -> 트리종료
            if(RootNode.state == Node.State.InProgress)
                TreeState = RootNode.Update();
            return TreeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            Undo.RecordObject(this, "Behavior Tree (CreateNode)");
            nodes.Add(node);

            if(!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (CreateNode)");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behavior Tree (DeleteNode)");
            nodes.Remove(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets(); 
        }

        public void AddChild(Node parentNode, Node childNode)
        {
            RootNode rootNode = parentNode as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behavior Tree (AddChild)");
                rootNode.Child = childNode;
                EditorUtility.SetDirty(rootNode);
                return;
            }

            DecoratorNode decorator = parentNode as DecoratorNode;
            if(decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (AddChild)");
                decorator.Child = childNode;
                EditorUtility.SetDirty(decorator);
                return;
            }

            CompositeNode composite = parentNode as CompositeNode;
            if(composite)
            { 
                Undo.RecordObject(composite, "Behavior Tree (AddChild)");
                composite.Childs.Add(childNode);
                EditorUtility.SetDirty(composite);
                return;
            }
        }
        public void RemoveChild(Node parentNode, Node childNode)
        {
            RootNode rootNode = parentNode as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behavior Tree (RemoveChild)");
                rootNode.Child = null;
                EditorUtility.SetDirty(rootNode);
                return;
            }

            DecoratorNode decorator = parentNode as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (RemoveChild)");
                decorator.Child = null;
                EditorUtility.SetDirty(decorator);
                return;
            }

            CompositeNode composite = parentNode as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behavior Tree (RemoveChild)");
                composite.Childs.Remove(childNode);
                EditorUtility.SetDirty(composite);
                return;
            }
        }
        public List<Node> GetChilds(Node parentNode)
        {
            List<Node> tempList = new List<Node>();

            RootNode rootNode = parentNode as RootNode;
            if (rootNode)
            {
                if (rootNode.Child)
                {
                    tempList.Add(rootNode.Child);
                }
            }


            DecoratorNode decorator = parentNode as DecoratorNode;
            if (decorator)
            {
                if(decorator.Child)
                {
                    tempList.Add(decorator.Child);
                }
            }

            CompositeNode composite = parentNode as CompositeNode;
            if (composite)
            {
                tempList = composite.Childs;
            }
            return tempList;
        }

        public void BindBlackBoard(Blackboard bBoard)
        {
            Traverse(RootNode, node =>
            {
                BT.DecoratorNode dNode = node as DecoratorNode;
                if (dNode)
                    dNode.bBoard = bBoard;

                BT.ServiceNode sNode = node as ServiceNode;
                if (sNode)
                    sNode.bBoard = bBoard;

            });
        }
    }
}
