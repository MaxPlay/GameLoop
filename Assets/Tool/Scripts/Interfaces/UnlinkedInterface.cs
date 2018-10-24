using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameLoop.Interfaces
{
    public abstract class UnlinkedInterface : MonoBehaviour
    {
        private static Type baseType;

        public static List<Type> AssignableTypes { get; private set; }
        public static string[] AssignableTypeNames { get; private set; }

        static UnlinkedInterface()
        {
            baseType = typeof(UnlinkedInterface);
            AssignableTypes = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract).OrderBy(t => t.Name).ToList();
            AssignableTypeNames = AssignableTypes.Select(t => t.Name).ToArray();
        }

        public abstract void Display(bool display);

        public delegate void UnlinkedInterfaceEventHandler(UnlinkedInterface ui);

        public event UnlinkedInterfaceEventHandler Destroyed;

        protected virtual void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}
