using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BasicComponentsPack.InternalClasses;
using System;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for DebugConsole.xaml
    /// </summary>
    public partial class DebugConsole
    {
        #region ErrorException
        public static readonly RoutedEvent ErrorExceptionEvent = EventManager.RegisterRoutedEvent("ErrorException",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DebugConsole));

        public event RoutedEventHandler ErrorException
        {
            add { AddHandler(ErrorExceptionEvent, value); }
            remove { RemoveHandler(ErrorExceptionEvent, value); }
        }
        #endregion

        #region OpenSolution
        public static readonly RoutedEvent OpenSolutionEvent = EventManager.RegisterRoutedEvent("OpenSolution",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DebugConsole));

        public event RoutedEventHandler OpenSolution
        {
            add { AddHandler(OpenSolutionEvent, value); }
            remove { RemoveHandler(OpenSolutionEvent, value); }
        }
        #endregion

        #region CompileSolution
        public static readonly RoutedEvent CompileSolutionEvent = EventManager.RegisterRoutedEvent("CompileSolution",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DebugConsole));

        public event RoutedEventHandler CompileSolution
        {
            add { AddHandler(CompileSolutionEvent, value); }
            remove { RemoveHandler(CompileSolutionEvent, value); }
        }
        #endregion

        readonly DebugConsoleVM _dc = new DebugConsoleVM();

        private readonly Dictionary<string, Action<string>> _commandExecuter;
        private void PerformExecuter(string command)
        {
            var commandKeys = command.Split(' ');
            if (_commandExecuter.ContainsKey(commandKeys[0]))
            {
                _commandExecuter[commandKeys[0]](command);
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, String.Format("Not found command {0}", commandKeys[0])));
            }
        }

        public void DefineExecuter(string command, Action<string> executer)
        {
            try
            {
                if (_commandExecuter.ContainsKey(command))
                {
                    _commandExecuter[command] = executer;
                }
                else
                {
                    _commandExecuter.Add(command, executer);
                }
            }
            catch (Exception e)
            {
                //RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, e.Message));
            }
        }

        public void ErrorListener(object sender, RoutedEventArgs e)
        {
            _dc.ConsoleOutput.Add(e.OriginalSource.ToString());
            Scroller.ScrollToBottom();
        }

        private void CloseCommand(string command=null)
        {
            Application.Current.Shutdown();
        }

        private void CompileCommand(string command=null)
        {
            command = command.Split(' ')[1];
            RaiseEvent(new RoutedEventArgs(CompileSolutionEvent, command));
        }

        private void OpenSolutionCommand(string pathToSolution = null)
        {
            pathToSolution = pathToSolution.Split(' ')[1];
            RaiseEvent(new RoutedEventArgs(OpenSolutionEvent, pathToSolution));
        }
        public DebugConsole()
        {
            InitializeComponent();
            DataContext = _dc;
            Loaded += MainWindow_Loaded;
            _commandExecuter = new Dictionary<string, Action<string>>()
            {
                {"close", CloseCommand},
                {"open", OpenSolutionCommand},
                {"compile", CompileCommand}
            };
        }

        

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InputBlock.KeyDown += InputBlock_KeyDown;
        }

        void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _dc.ConsoleInput = InputBlock.Text;
                _dc.ConsoleOutput.Add(_dc.ConsoleInput);
                PerformExecuter(_dc.ConsoleInput);
                _dc.ConsoleInput = String.Empty;
                InputBlock.Focus();
                Scroller.ScrollToBottom();
            }
        }
    }
}
