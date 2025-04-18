using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace Incredimaker
{
    public class TestLuaManager : MonoBehaviour
    {
        // The LUA Script
        string scriptCode = @"
        function Start()
            log('Hello! I am LUA')
        end
        ";

        // Print Function for LUA to use
        void LuaPrint(string text)
        {
            Debug.Log(text);
        }

        void BasicLuaFunctionCall(string functionName)
        {
            // Call the function
            DynValue function = script.Globals.Get(functionName);
            if (function != null && function.Type == DataType.Function)
            {
                script.Call(function);
            }
        }

        // Initializing
        Script script = new Script();

        void Awake()
        {
            // Set up LUA API
            script.Globals["log"] = (Action<string>)LuaPrint;

            // Start the LUA script
            script.DoString(scriptCode);

            BasicLuaFunctionCall("Awake");
        }

        // Start is called before the first frame update
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
