using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WhiteMagic.Internals;

namespace cleanCore
{
    
    public static class Events
    {
        private const int RegisterCheckWait = 500; /*ms*/

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int LuaFunctionDelegate(IntPtr luaState);

        private static DateTime _lastRegisterCheck = DateTime.Now - TimeSpan.FromMilliseconds(RegisterCheckWait);
        private static Detour _eventDetour;
        private static readonly Dictionary<string, List<EventHandler>> _eventHandler = new Dictionary<string, List<EventHandler>>();

        public delegate void EventHandler(string eventName, List<string> args);

        public static void Initialize()
        {
            var eventVictim = Helper.Magic.RegisterDelegate<LuaFunctionDelegate>(Offsets.EventVictim);
            _eventDetour = Helper.Magic.Detours.CreateAndApply(eventVictim, new LuaFunctionDelegate(HandleVictimCall), "EventVictim");
        }

        public static void Pulse()
        {
            if ((DateTime.Now - _lastRegisterCheck).TotalMilliseconds >= RegisterCheckWait)
            {
                _lastRegisterCheck = DateTime.Now;
                if (!ListenerExists)
                    ExecuteIngameListener();
            }
        }

        public static void Register(string name, EventHandler handler)
        {
            if (_eventHandler.ContainsKey(name))
                _eventHandler[name].Add(handler);
            else
                _eventHandler.Add(name, new List<EventHandler> {handler});
        }

        public static void Remove(string name, EventHandler handler)
        {
            if (_eventHandler.ContainsKey(name))
                _eventHandler[name].Remove(handler);
        }
        
        private static void HandleEvent(List<string> args)
        {
            string eventName = args[0];
            args.RemoveAt(0);
            if (_eventHandler.ContainsKey(eventName))
            {
                foreach (var handler in _eventHandler[eventName])
                    handler(eventName, args);
            }
        }

        private static int HandleVictimCall(IntPtr luaState)
        {
            int top = LuaInterface.GetTop(luaState);
            if (top > 0)
            {
                var args = new List<string>(top);
                for (int i = 1; i <= top; i++)
                    args.Add(LuaInterface.StackObjectToString(luaState, i));
                LuaInterface.Pop(luaState, top);
                HandleEvent(args);
            }
            else
            {
                // legal call
                return (int)_eventDetour.CallOriginal(luaState);
            }

            return 0;
        }

        private static bool ListenerExists
        {
            get
            {
                const string checkCommand = "evcFrame == nil";
                var ret = WoWScript.Execute(checkCommand);
                return ret[0] == "false";
            }
        }

        private static void ExecuteIngameListener()
        {
            const string command =
                "local frame = CreateFrame('Frame', 'evcFrame'); frame:RegisterAllEvents(); frame:SetScript('OnEvent', function(self, event, ...) GetBillingTimeRested(event, ...); end);";
            WoWScript.ExecuteNoResults(command);
        }
    }

}