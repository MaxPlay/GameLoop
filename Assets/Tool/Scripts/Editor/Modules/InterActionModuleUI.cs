using GameLoop.Modules;
using UnityEngine;

namespace GameLoop.Editor.Modules
{
    public class InterActionModuleUI : ActionModuleUI
    {
        public InterActionModuleUI(InterActionModule module) : base(module)
        {
        }

        public override Color Background
        {
            get
            {
                return ModuleSystemPreferences.InterActionColor;
            }
        }
    }
}
