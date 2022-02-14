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
        Debug.Log("clicked");
        
        BT_Key.KeyType type = (BT_Key.KeyType)keykind.value;
        
        CreateKey(keyName.value.ToString(), type);

    }
    public void Populateboard(Blackboard board)
    {
        bBoard = board;
        BindElement();
        Debug.Log(bBoard.bb_keys.Count);

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
        Debug.Log("createkeyview");
        switch (type)
        {
            case BT_Key.KeyType.E_bool:
                {
                    Toggle toggleField = new Toggle(name);
                    keycontainer.Add(toggleField);
                }
                break;
            case BT_Key.KeyType.E_int:
                {
                    IntegerField intField = new IntegerField(name);
                    keycontainer.Add(intField);
                }
                break;
            case BT_Key.KeyType.E_float:
                {
                    FloatField floatField = new FloatField(name);
                    keycontainer.Add(floatField);
                }
                break;
            case BT_Key.KeyType.E_vector2:
                {
                    Vector2Field vector2Field = new Vector2Field(name);
                    keycontainer.Add(vector2Field);
                }
                break;
            case BT_Key.KeyType.E_gameobject:
                {
                    ObjectField objField = new ObjectField(name);
                    keycontainer.Add(objField);
                }
                break;
        }
    }
    public void CreateKey(string name, BT_Key.KeyType type, object val = null)
    {
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
