using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class BlackboardView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<BlackboardView, UxmlTraits> { }

    Blackboard bBoard;
    VisualElement keycontainer;
    EnumField keykind;
    TextField keyName;
    Button AddButton;
    Action clicked;
    public BlackboardView()
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BlackboardEditor.uss");
        styleSheets.Add(styleSheet);
    }
    public void BindElement()
    {
        keycontainer = this.Query<VisualElement>("KeyContainer");
        keykind = this.Q<EnumField>();
        keyName = this.Q<TextField>();
        AddButton = this.Q<Button>();
        if(keycontainer == null || keykind == null || keyName == null || AddButton == null)
        {
            Debug.Log("bind cannot");
        }
        AddButton.clicked += OnAddButtonClicked;
    }
    private void OnAddButtonClicked()
    {
        BT_Key.KeyType type = (BT_Key.KeyType)keykind.value;
        
        CreateKey(keyName.value.ToString(), type);

    }
    public void Populateboard(Blackboard board)
    {
        bBoard = board;
        BindElement();

        bBoard.bb_keys.Keys.ToList().ForEach((key) =>
        {
            object obj;
            if(bBoard.bb_keys.TryGetValue(key, out obj))
            {
                if(obj is bool)
                {
                    CreateKeyView(key, BT_Key.KeyType.E_bool, obj);
                }
                else if (obj is GameObject)
                {
                    CreateKeyView(key, BT_Key.KeyType.E_gameobject, obj);
                }
                else if (obj is Vector2)
                {
                    CreateKeyView(key, BT_Key.KeyType.E_vector2, obj);
                }
                else if(obj is int)
                {
                    CreateKeyView(key, BT_Key.KeyType.E_int, obj);
                }
                else if(obj is float)
                {
                    CreateKeyView(key, BT_Key.KeyType.E_float, obj);
                }
                else
                {
                    Debug.Log(key.GetType().Name);
                }
            }
            
        });
    }
    public void CreateKeyView(string name, BT_Key.KeyType type, object val = null)
    {
        switch (type)
        {
            case BT_Key.KeyType.E_bool:
                {
                    BlackboardKeyView keyView = new BlackboardKeyView();
                    keyView.GenerateKeyView("Bool", name);
                    keycontainer.Add(keyView);
                }
                break;
            case BT_Key.KeyType.E_int:
                {
                    BlackboardKeyView keyView = new BlackboardKeyView();
                    keyView.GenerateKeyView("Int", name);
                    keycontainer.Add(keyView);
                }
                break;
            case BT_Key.KeyType.E_float:
                {
                    BlackboardKeyView keyView = new BlackboardKeyView();
                    keyView.GenerateKeyView("Float", name);
                    keycontainer.Add(keyView);
                }
                break;
            case BT_Key.KeyType.E_vector2:
                {
                    BlackboardKeyView keyView = new BlackboardKeyView();
                    keyView.GenerateKeyView("Vec2", name);
                    keycontainer.Add(keyView);
                }
                break;
            case BT_Key.KeyType.E_gameobject:
                {
                    BlackboardKeyView keyView = new BlackboardKeyView();
                    keyView.GenerateKeyView("Object", name);
                    keycontainer.Add(keyView);
                }
                break;
        }
    }
    public void CreateKey(string name, BT_Key.KeyType type, object val = null)
    {
        EditorUtility.SetDirty(bBoard);
        switch (type)
        {
            case BT_Key.KeyType.E_bool:
                {
                    if (val == null)
                        bBoard.bb_keys.Add(name, false);
                    else
                        bBoard.bb_keys.Add(name, val);
                }
                break;
            case BT_Key.KeyType.E_int:
                {
                    if (val == null)
                        bBoard.bb_keys.Add(name, 0);
                    else
                        bBoard.bb_keys.Add(name, val);
                }
                break;
            case BT_Key.KeyType.E_float:
                {
                    if (val == null)
                        bBoard.bb_keys.Add(name, 0.0f);
                    else
                        bBoard.bb_keys.Add(name, val);
                }
                break;
            case BT_Key.KeyType.E_vector2:
                {
                    if (val == null)
                        bBoard.bb_keys.Add(name, new Vector2(0,0));
                    else
                        bBoard.bb_keys.Add(name, val);
                }
                break;
            case BT_Key.KeyType.E_gameobject:
                {
                    if (val == null)
                        bBoard.bb_keys.Add(name, new GameObject());
                    else
                        bBoard.bb_keys.Add(name, val);
                }
                break;
        }
        AssetDatabase.SaveAssets();

        CreateKeyView(name, type, val);
    }
}
