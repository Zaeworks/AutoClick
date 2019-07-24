using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaScript.Abstract;
using GalaScript;
using WindowsInput;
using System.Windows;

namespace AutoClick
{
    public class Simulator
    {

        private IEngine _engine;
        private IInputSimulator _input;
        private CancellationToken _token;

        private double ScreenW => SystemParameters.PrimaryScreenWidth;
        private double ScreenH => SystemParameters.PrimaryScreenHeight;

        public static void Bind(IEngine engine, CancellationToken token)
        {
            var simulator = new Simulator()
            {
                _engine = engine,
                _input = new InputSimulator(),
                _token = token
            };

            engine.Register("move", (Action<int, int>)simulator.Move);
            engine.Register("click", (Action)simulator.Click);
            engine.Register("sleep", (Action<int>)simulator.Sleep);
        }

        public void Move(int x, int y)
        {
            var _x = x * (65535 / ScreenW);
            var _y = y * (65535 / ScreenH);
            _input.Mouse.MoveMouseTo(_x, _y);
        }

        public void Click()
        {
            _input.Mouse.LeftButtonDown();
            _input.Mouse.Sleep(10);
            _input.Mouse.LeftButtonUp();
        }

        public void Sleep(int milliseconds)
        {
            try
            {
                Task.Delay(milliseconds).Wait(_token);
            }
            catch(OperationCanceledException)
            {
                return;
            }
        }
    }
}
