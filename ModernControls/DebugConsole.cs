﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using ModernControls.InternalClasses;

namespace ModernControls
{
    public class DebugConsole : Control
    {
        private List<Log> AllLogs { get; set; } 

        private LogType _currentLogType;

        public LogType CurrentLogType
        {
            get { return _currentLogType; }
            set
            {
                _currentLogType = value;
                SetCurrentLogs();
            }
        }

        private void SetCurrentLogs()
        {
            if (CurrentLogType != LogType.Default)
            {
                var Logs = AllLogs.FindAll(x => x.Type == CurrentLogType);
                (GetTemplateChild("DebugList") as ListBox).ItemsSource = Logs;
            }
            else
            {
                (GetTemplateChild("DebugList") as ListBox).ItemsSource = AllLogs;
            }
        }

        static DebugConsole()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DebugConsole), new FrameworkPropertyMetadata(typeof(DebugConsole)));
        }

        public override void OnApplyTemplate()
        {
            AllLogs= new List<Log>();
            base.OnApplyTemplate();
        }

        public void WriteLog(string message, LogType type)
        {
            AllLogs.Add(new Log { Date = new DateTime(), Text = message, Type = type });
            SetCurrentLogs();
        }
    }
}