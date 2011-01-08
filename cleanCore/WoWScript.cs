using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace cleanCore
{
    
    public static class WoWScript
    {

        private static string PopError(IntPtr state)
        {
            var p = LuaInterface.ToLString(state, 1, 0);
            if (p == IntPtr.Zero)
                return "Unknown Error";
            LuaInterface.Pop(state, 1);
            return Marshal.PtrToStringAnsi(p);
        }

        public static void ExecuteNoResults(string query)
        {
            ExecuteInternal(query, false);
        }

        public static List<string> Execute(string query)
        {
            return ExecuteInternal(query, true);
        }

        private static List<string> ExecuteInternal(string query, bool withResults)
        {
            if (withResults)
                query = "return " + query;
            var state = LuaInterface.LuaState;
            int top = LuaInterface.GetTop(state);

            var data = Encoding.ASCII.GetBytes(query);
            var memory = Marshal.AllocHGlobal(data.Length + 1);
            try
            {
                Marshal.Copy(data, 0, memory, data.Length);
                Marshal.WriteByte(memory + data.Length, 0);

                if (LuaInterface.LoadBuffer(state, memory, data.Length, "cleanCore") > 0)
                    return new List<string> {PopError(state)};

                if (LuaInterface.PCall(state, 0, withResults ? (int)LuaInterface.LuaConstant.MultRet : 0, 0) > 0)
                    return new List<string> {PopError(state)};

                int returnValueCount = LuaInterface.GetTop(state) - top;
                var ret = new List<string>(returnValueCount);
                for (int i = 1; i <= returnValueCount; i++)
                    ret.Add(LuaInterface.StackObjectToString(state, i));
                LuaInterface.Pop(state, returnValueCount);
                return ret;
            }
            finally
            {
                Marshal.FreeHGlobal(memory);
            }
        }
    }

}