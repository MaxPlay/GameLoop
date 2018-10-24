using System;
using UnityEngine;

namespace GameLoop.Editor.Extensions
{
    public static class ColorExtension
    {
        #region Public Methods

        public static Color ToColor(this Int32 value)
        {
            byte a = (byte)(value >> 24);
            byte r = (byte)(value >> 16);
            byte g = (byte)(value >> 8);
            byte b = (byte)(value >> 0);
            return new Color32(r, g, b, a);
        }

        public static Color32 ToColor32(this Int32 value)
        {
            byte a = (byte)(value >> 24);
            byte r = (byte)(value >> 16);
            byte g = (byte)(value >> 8);
            byte b = (byte)(value >> 0);
            return new Color32(r, g, b, a);
        }

        public static int ToInt32(this Color colorF)
        {
            Color32 color = colorF;
            return ((color.a << 24) | (color.r << 16) |
                          (color.g << 8) | (color.b << 0));
        }

        public static int ToInt32(this Color32 color)
        {
            return ((color.a << 24) | (color.r << 16) |
                          (color.g << 8) | (color.b << 0));
        }

        #endregion Public Methods
    }
}