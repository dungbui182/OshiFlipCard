#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN) && !UNITY_WEBGL

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GrandDreams.Core.Utilities;

namespace GrandDreams.Core
{
    public class AlwaysOnTop : MonoBehaviour
    {
        #region WIN32API

        public static readonly System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
        public static readonly System.IntPtr HWND_NOT_TOPMOST = new System.IntPtr(-2);
        const System.UInt32 SWP_SHOWWINDOW = 0x0040;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rect r)
                : this((int)r.xMin, (int)r.yMin, (int)r.xMax, (int)r.yMax)
            {
            }

            public int X
            {
                get
                {
                    return Left;
                }
                set
                {
                    Right -= (Left - value);
                    Left = value;
                }
            }

            public int Y
            {
                get
                {
                    return Top;
                }
                set
                {
                    Bottom -= (Top - value);
                    Top = value;
                }
            }

            public int Height
            {
                get
                {
                    return Bottom - Top;
                }
                set
                {
                    Bottom = value + Top;
                }
            }

            public int Width
            {
                get
                {
                    return Right - Left;
                }
                set
                {
                    Right = value + Left;
                }
            }

            public static implicit operator Rect(RECT r)
            {
                return new Rect(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rect r)
            {
                return new RECT(r);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern System.IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool DrawMenuBar(IntPtr hWnd);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        #region Declare Variables

        const int GWL_STYLE = -16;
        const int WS_BORDER = 1;

        private bool keepGameObjectOnLoad = false;
        public bool KeepGameObjectOnLoad
        {
            set
            {
                keepGameObjectOnLoad = value;
                if (keepGameObjectOnLoad)
                {
                    GameObject.DontDestroyOnLoad(this.gameObject);
                }
            }
            get
            {
                return keepGameObjectOnLoad;
            }
        }

        private static AlwaysOnTop instance = null;
        public static AlwaysOnTop Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("AlwaysOnTop");
                    instance = go.AddComponent<AlwaysOnTop>();
                }
                else
                {
                    if (instance.gameObject.IsDestroyed())
                    {
                        instance = null;
                        return Instance;
                    }
                }

                return instance;
            }
        }
#if !UNITY_EDITOR
        private bool fullScreen = false;
        private string windowTitle = "";
#endif
        private int preferPosX = 0;
        private int preferPosY = 0;
        private int preferSizeWidth = 1920;
        private int preferSizeHeight = 1080;

        private bool allowToSwitchScreen = false;

#endregion

        // Use this for initialization
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
#if !UNITY_EDITOR
            AssignTopmostWindow(windowTitle, true);
#endif
        }

        private void Update()
        {
#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F7))
        {
            fullScreen = !fullScreen;

            System.IntPtr hWnd = FindWindow((string)null, windowTitle);
            RECT rect = new RECT();
            GetWindowRect(new HandleRef(this, hWnd), out rect);

            if (fullScreen)
            {
                Screen.SetResolution(rect.Width, rect.Height, fullScreen);
            }
            else
            {
                Screen.SetResolution(preferSizeWidth, preferSizeHeight, fullScreen);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            AssignTopmostWindow(windowTitle, true);
        }
        else if (Input.GetKeyDown(KeyCode.F9))
        {
            AssignTopmostWindow(windowTitle, false);
        }
#endif
        }

#region Public Function

        public void Initialize(string windowTitle, int preferPosX, int preferPosY, int preferSizeWidth, int preferSizeHeight, bool allowToSwitchScreen = true)
        {
            #if !UNITY_EDITOR
                this.windowTitle = windowTitle;
            #endif
            this.preferPosX = preferPosX;
            this.preferPosY = preferPosY;
            this.preferSizeWidth = preferSizeWidth;
            this.preferSizeHeight = preferSizeHeight;

            this.allowToSwitchScreen = allowToSwitchScreen;
        }

        public bool AssignTopmostWindow(string WindowTitle, bool MakeTopmost)
        {
            UnityEngine.Debug.Log("Assigning top most flag to window of title: " + WindowTitle);

            System.IntPtr hWnd = FindWindow((string)null, WindowTitle);

            SetWindowLong(hWnd, GWL_STYLE, WS_BORDER);

            RECT rect = new RECT();
            GetWindowRect(new HandleRef(this, hWnd), out rect);

            return SetWindowPos(hWnd, MakeTopmost ? HWND_TOPMOST : HWND_NOT_TOPMOST, preferPosX, preferPosY, preferSizeWidth, preferSizeHeight, SWP_SHOWWINDOW);
        }

        public bool IsNullOrWhitespace(string Str)
        {
            if (Str.Equals("null"))
            {
                return true;
            }
            foreach (char c in Str)
            {
                if (c != ' ')
                {
                    return false;
                }
            }
            return true;
        }

#endregion

        private string[] GetWindowTitles()
        {
            List<string> WindowList = new List<string>();

            Process[] ProcessArray = Process.GetProcesses();
            foreach (Process p in ProcessArray)
            {
                if (!IsNullOrWhitespace(p.MainWindowTitle))
                {
                    WindowList.Add(p.MainWindowTitle);
                }
            }

            return WindowList.ToArray();
        }
    }
}
#endif