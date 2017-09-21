﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace WeersProductions
{
    public class VerticalBlock : IDisposable
    {
        public VerticalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public VerticalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }

    public class ScrollviewBlock : IDisposable
    {
        public ScrollviewBlock(ref Vector2 scrollPos, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, options);
        }

        public void Dispose()
        {
            GUILayout.EndScrollView();
        }
    }

    public class HorizontalBlock : IDisposable
    {
        public HorizontalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public HorizontalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }

    public class ColoredBlock : System.IDisposable
    {
        public ColoredBlock(Color color)
        {
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = Color.white;
        }
    }

    [Serializable]
    public class TabsBlock
    {
        private readonly Dictionary<string, Action> _methods;
        private Action _currentGuiMethod;
        public int CurMethodIndex = -1;

        public TabsBlock(Dictionary<string, Action> methods)
        {
            this._methods = methods;
            SetCurrentMethod(0);
        }

        public void Draw()
        {
            var keys = _methods.Keys.ToArray();
            using (new VerticalBlock(EditorStyles.helpBox))
            {
                using (new HorizontalBlock())
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        var btnStyle = i == 0
                            ? EditorStyles.miniButtonLeft
                            : i == (keys.Length - 1)
                                ? EditorStyles.miniButtonRight
                                : EditorStyles.miniButtonMid;
                        using (new ColoredBlock(_currentGuiMethod == _methods[keys[i]] ? Color.grey : Color.white))
                            if (GUILayout.Button(keys[i], btnStyle))
                                SetCurrentMethod(i);
                    }
                }
                GUILayout.Label(keys[CurMethodIndex], EditorStyles.centeredGreyMiniLabel);
                _currentGuiMethod();
            }
        }

        public void SetCurrentMethod(int index)
        {
            CurMethodIndex = index;
            _currentGuiMethod = _methods[_methods.Keys.ToArray()[index]];
        }
    }
}