using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Scripter
{
    public class Script
    {
        private readonly List<IScriptAction> actions = new List<IScriptAction>();

        public Script(string text)
        {
            string[] strArray = text.Split(new[] {';', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(new[] {','});
                if ((strArray2.Length == 2) && !strArray2[0].StartsWith("#"))
                {
                    int num2;
                    if (strArray2[0].StartsWith("MOUSE", true, CultureInfo.CurrentCulture))
                    {
                        int mouseButton = int.Parse(strArray2[0][5].ToString());
                        actions.Add(new MousePress(mouseButton));
                        num2 = int.Parse(strArray2[1]);
                        actions.Add(new Delay(num2));
                    }
                    else
                    {
                        uint num3;
                        try
                        {
                            num3 = (uint) ((Keys) Enum.Parse(typeof (Keys), strArray2[0]));
                        }
                        catch (ArgumentException)
                        {
                            throw new InvalidOperationException("No code for key '" + strArray2[0] + '"');
                        }
                        actions.Add(new KeyPress(num3));
                        num2 = int.Parse(strArray2[1]);
                        actions.Add(new Delay(num2));
                    }
                }
            }
        }

        public IEnumerable<IScriptAction> Actions
        {
            get { return actions; }
        }

        public bool IsFinished
        {
            get { return false; }
        }

        #region Nested type: Delay

        public class Delay : IScriptAction
        {
            private readonly int time;

            public Delay(int time)
            {
                this.time = time;
            }

            #region IScriptAction Members

            public void Do(uint targetWindow)
            {
                Thread.Sleep(time);
            }

            #endregion
        }

        #endregion

        #region Nested type: KeyPress

        public class KeyPress : IScriptAction
        {
            private readonly uint code;

            public KeyPress(uint code)
            {
                this.code = code;
            }

            #region IScriptAction Members

            public void Do(uint targetWindow)
            {
                Funcs.SendKeyToWindow(targetWindow, code);
            }

            #endregion
        }

        #endregion

        #region Nested type: MousePress

        public class MousePress : IScriptAction
        {
            private readonly int mouseButton;

            public MousePress(int mouseButton)
            {
                this.mouseButton = mouseButton;
            }

            #region IScriptAction Members

            public void Do(uint targetWindow)
            {
                try
                {
                    Funcs.MouseClick(targetWindow, mouseButton);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: IScriptAction

        public interface IScriptAction
        {
            void Do(uint targetWindow);
        }

        #endregion
    }
}