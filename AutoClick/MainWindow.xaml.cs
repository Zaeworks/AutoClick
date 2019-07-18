using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaScript;


namespace AutoClick
{
    public partial class MainWindow : Window
    {

        private ScriptEngine _engine;
        private Task _task;
        private CancellationTokenSource _cts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Reset_Click(sender, e);

            try
            {
                _cts = new CancellationTokenSource();
                _engine = new ScriptEngine(true);
                Simulator.Bind(_engine, _cts.Token);
                _engine.Prepare(textBox.Text);
                _task = Task.Run(() => { _engine.Run(); }, _cts.Token);
            }
            catch(Exception ex)
            {
                label.Content = ex.GetType().Name;
                Debug.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if(_task != null)
            {
                _engine.Paused = true;
                _cts.Cancel();
                _task = null;
            }
        }
    }
}
