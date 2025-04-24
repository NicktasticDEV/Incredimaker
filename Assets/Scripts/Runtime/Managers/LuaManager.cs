using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace Incredimaker
{
    public class LuaManager : MonoBehaviour
    {
        [Multiline]
        public string script = @"";

        // MoonSharp initialization stuff
        Script scriptExe = new Script();


        #region Lua Functions

        void LuaPrint(string text)
        {
            Debug.Log(text);
        }

        #endregion


        // Utilities to set up stuff
        void BasicLuaFunctionCall(string functionName)
        {
            // Call the function
            DynValue function = scriptExe.Globals.Get(functionName);
            if (function != null && function.Type == DataType.Function)
            {
                scriptExe.Call(function);
            }
        }

        void Awake()
        {
            // Set up the functions to be usable
            scriptExe.Globals["Log"] = (Action<string>)LuaPrint;

            // Start the LUA script
            scriptExe.DoString(script);
            BasicLuaFunctionCall("Awake");
        }

        void Start()
        {
            BasicLuaFunctionCall("Start");
        }

        void Update()
        {
            BasicLuaFunctionCall("Update");
        }
    }
}