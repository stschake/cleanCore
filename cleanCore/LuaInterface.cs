using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace cleanCore
{

    internal static class LuaInterface
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaGetTopDelegate(IntPtr luaState);
        public static LuaGetTopDelegate GetTop;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LuaSetTopDelegate(IntPtr luaState, int index);
        public static LuaSetTopDelegate SetTop;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaTypeDelegate(IntPtr luaState, int index);
        public static LuaTypeDelegate Type;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr LuaToLStringDelegate(IntPtr luaState, int index, int zero);
        public static LuaToLStringDelegate ToLString;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaToBooleanDelegate(IntPtr luaState, int index);
        public static LuaToBooleanDelegate ToBoolean;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate double LuaToNumberDelegate(IntPtr luaState, int index);
        public static LuaToNumberDelegate ToNumber;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaPCallDelegate(IntPtr luaState, int nargs, int nresults, int errfunc);
        public static LuaPCallDelegate PCall;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int LuaLoadBufferDelegate(
            IntPtr luaState, IntPtr buffer, int bufferLength,
            [MarshalAs(UnmanagedType.LPStr)] string chunkName);
        public static LuaLoadBufferDelegate LoadBuffer;

        public static void Pop(IntPtr state, int n)
        {
            SetTop(state, -(n) - 1);
        }

        public enum LuaConstant
        {
            MultRet = -1,
            TypeNil = 0,
            TypeBoolean = 1,
            TypeNumber = 3,
            TypeString = 4
        }

        public static IntPtr LuaState
        {
            get
            {
                return Helper.Magic.Read<IntPtr>(Offsets.LuaState);
            }
        }

        public static void Initialize()
        {
            GetTop = Helper.Magic.RegisterDelegate<LuaGetTopDelegate>(Offsets.LuaGetTop);
            SetTop = Helper.Magic.RegisterDelegate<LuaSetTopDelegate>(Offsets.LuaSetTop);
            Type = Helper.Magic.RegisterDelegate<LuaTypeDelegate>(Offsets.LuaType);
            ToLString = Helper.Magic.RegisterDelegate<LuaToLStringDelegate>(Offsets.LuaToLString);
            ToBoolean = Helper.Magic.RegisterDelegate<LuaToBooleanDelegate>(Offsets.LuaToBoolean);
            ToNumber = Helper.Magic.RegisterDelegate<LuaToNumberDelegate>(Offsets.LuaToNumber);
            PCall = Helper.Magic.RegisterDelegate<LuaPCallDelegate>(Offsets.LuaPCall);
            LoadBuffer = Helper.Magic.RegisterDelegate<LuaLoadBufferDelegate>(Offsets.LuaLoadBuffer);
        }

        public static string StackObjectToString(IntPtr state, int index)
        {
            var ltype = (LuaConstant)Type(state, index);

            switch (ltype)
            {
                case LuaConstant.TypeNil:
                    return "nil";

                case LuaConstant.TypeBoolean:
                    return ToBoolean(state, index) > 0 ? "true" : "false";

                case LuaConstant.TypeNumber:
                    return ToNumber(state, index).ToString(CultureInfo.InvariantCulture);

                case LuaConstant.TypeString:
                    return Marshal.PtrToStringAnsi(ToLString(state, index, 0));

                default:
                    return "<unknown lua type>";
            }
        }
    }

}