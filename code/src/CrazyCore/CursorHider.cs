﻿using Engine;
using Engine.Behaviors;
using Engine.Graphics;
using System.Numerics;
using Veldrid.Platform;
using System.Runtime.InteropServices;

namespace CrazyCore
{
    public class CursorHider : Behavior
    {
        private GraphicsSystem _gs;
        private InputSystem _input;

        public bool CursorVisible { get; set; } = false;
        public bool ForceCenter { get; set; } = true;

        protected override void Start(SystemRegistry registry)
        {
            base.Start(registry);
            _gs = registry.GetSystem<GraphicsSystem>();
            _input = registry.GetSystem<InputSystem>();
        }

        protected override void PostDisabled()
        {
            if (_gs != null && _gs.Context.Window.Exists)
            {
                _gs.Context.Window.CursorVisible = true;
            }
        }

        public override void Update(float deltaSeconds)
        {
            Window window = _gs.Context.Window;
            if (window.Exists && window.Focused)
            {
                if (_input.GetKeyDown(Key.Escape))
                {
                    CursorVisible = !CursorVisible;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    window.CursorVisible = true;
                }
                bool currentVisibility = CursorVisible || MenuGlobals.NumMenusOpen > 0;
                if (ForceCenter && !currentVisibility)
                {
                    _input.MousePosition = new Vector2(window.Width / 2f, window.Height / 2f);
                }
                window.CursorVisible = currentVisibility;
            }
        }
    }
}
