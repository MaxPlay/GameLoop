using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameLoop.Interfaces;
using GameLoop.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLoop.Data
{
    [CreateAssetMenu(menuName = MenuStructure.MAIN_DIRECTORY + MenuStructure.INTERFACE_DIRECTORY + "InterfaceControl")]
    public class InterfaceControl : GameLoopScriptableObject
    {
        [SerializeField]
        EventSystem eventSystem;
        [SerializeField]
        UserInterface[] interfaces;

        Dictionary<Type, UnlinkedInterface> unlinkedInterfaceCache;

        static InterfaceControl()
        {
            Type baseType = typeof(InterfaceControl);
            DisplayUIReflection = baseType.GetMethod("DisplayUI");
            GetUnlinkedInterfaceReflection = baseType.GetMethod("GetUnlinkedInterface");
            GetInterfaceReflection = baseType.GetMethod("GetInterface");
        }

        public static MethodInfo DisplayUIReflection { get; private set; }
        public static MethodInfo GetUnlinkedInterfaceReflection { get; private set; }
        public static MethodInfo GetInterfaceReflection { get; private set; }

        public void DisplayUI<T>(bool display) where T : UnlinkedInterface
        {
            T ui = GetUnlinkedInterface<T>();
            if (ui == null)
                return;

            ui.Display(display);
        }

        [SerializeField]
        [HideInInspector]
        ModuleSystem system;

        Dictionary<Type, UserInterface> interfaceLookup;

        public T GetUnlinkedInterface<T>() where T : UnlinkedInterface
        {
            if (unlinkedInterfaceCache.ContainsKey(typeof(T)))
                return unlinkedInterfaceCache[typeof(T)] as T;

            T ui = FindObjectOfType<T>();
            unlinkedInterfaceCache.Add(ui.GetType(), ui);
            ui.Destroyed += UI_Destroyed;
            return ui;
        }

        private void UI_Destroyed(UnlinkedInterface ui)
        {
            unlinkedInterfaceCache.Remove(ui.GetType());
            ui.Destroyed -= UI_Destroyed;
        }

        public new ModuleSystem System
        {
            get
            {
                return system;
            }
        }

        public Type[] InterfaceTypes { get; private set; }

        public string[] InterfaceTypeNames { get; private set; }

        public T GetInterface<T>() where T : UserInterface
        {
            Type type = typeof(T);
            if (!interfaceLookup.ContainsKey(type))
                return null;

            var ui = Instantiate(interfaceLookup[type]);
            ui.System = system;
            return ui as T;
        }

        public void Setup(ModuleSystem moduleSystem)
        {
            system = moduleSystem;
            if (interfaceLookup == null)
                interfaceLookup = new Dictionary<Type, UserInterface>();
            interfaceLookup.Clear();

            for (int i = 0; i < interfaces.Length; i++)
                interfaceLookup.Add(interfaces[i].GetType(), interfaces[i]);

            InterfaceTypes = interfaceLookup.Keys.OrderBy(t => t.Name).ToArray();
            InterfaceTypeNames = InterfaceTypes.Select(t => t.Name).ToArray();

            if (unlinkedInterfaceCache == null)
                unlinkedInterfaceCache = new Dictionary<Type, UnlinkedInterface>();
            unlinkedInterfaceCache.Clear();
        }
    }
}