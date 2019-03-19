﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Alturos.Yolo.Model;
using Rectangle = GameOverlay.Drawing.Rectangle;
// ReSharper disable IdentifierTypo

namespace NNHelper
{
    public static class Util
    {
        public static Rectangle GetEnemyBody(YoloItem nearestEnemy)
        {
            var nearestEnemyBody = Rectangle.Create(
                nearestEnemy.X + Convert.ToInt32(nearestEnemy.Width * (1f - GraphicsEx.BodyWidth) / 2f),
                y: nearestEnemy.Y + Convert.ToInt32(nearestEnemy.Height * (1f - GraphicsEx.BodyHeight) / 2f),
                Convert.ToInt32(GraphicsEx.BodyWidth * nearestEnemy.Width),
                Convert.ToInt32(GraphicsEx.BodyHeight * nearestEnemy.Height));
            return nearestEnemyBody;
        }

        public static Rectangle GetEnemyHead(YoloItem nearestEnemy)
        {
            var nearestEnemyHead = Rectangle.Create(
                nearestEnemy.X + Convert.ToInt32(nearestEnemy.Width * (1f - GraphicsEx.HeadWidth) / 2f),
                y: Convert.ToInt32(nearestEnemy.Y),
                Convert.ToInt32(GraphicsEx.HeadWidth * nearestEnemy.Width),
                Convert.ToInt32(GraphicsEx.HeadHeight * nearestEnemy.Height));
            return nearestEnemyHead;
        }

    }

    public static class VirtualMouse
    {
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void Move(int xDelta, int yDelta)
        {
            mouse_event(MOUSEEVENTF_MOVE, xDelta, yDelta, 0, 0);
        }

        public static void MoveTo(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | 0x4000, x, y, 0, 0);
        }

        public static void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static void LeftDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static void LeftUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static void RightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static void RightDown()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static void RightUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }
    }
}