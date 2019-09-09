using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF.Databases.Models;
using WPF.GUI.Pages;
using WPF.Models.Enums;
using WPF.Models.Interfaces;

namespace WPF.Managers
{
    public class NoteFiltersManager : IDisposableExtended
    {
        private NoteManagerPage page;
        private IBaseNoteManagerControl control;

        public bool IsDisposed { get; set; }

        public NoteFiltersManager(NoteManagerPage page, IBaseNoteManagerControl ctrl)
        {
            this.page = page;
            control = ctrl;
        }

        public void CreateFilterPanel(AitAccountModel account)
        {
            page.NoteManagerFilter.Items.Clear();
            var noteFilter = new MenuItem
            {
                Header = "Filters",
                Background = Brushes.Transparent,
                Foreground = Brushes.White
            };

            var obj = new MenuItem
            {
                Header = "All",
                Background = Brushes.LightGray,
                Foreground = Brushes.Black,
                IsEnabled = false
            };
            obj.Click += ALL_Click;
            noteFilter.Items.Add(obj);

            if (account != null)
            {
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.UNDEFINED))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.UNDEFINED)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += UNDEFINED_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.EXCEPTION))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.EXCEPTION)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += EXCEPTION_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.INFORMATION))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.INFORMATION)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += INFORMATION_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.NOTE))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.NOTE)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += NOTE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.TRACE))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.TRACE)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += TRACE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.QUERY))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.QUERY)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += QUERY_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.TASK))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.TASK)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += TASK_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.KEYLOGGER))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.KEYLOGGER)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += KEYLOGGER_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.ACTIVATION_CODE))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.ACTIVATION_CODE)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += ACTIVATION_CODE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.LOTTO_NOTE))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.LOTTO_NOTE)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += LOTTO_NOTE_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.DETACHED))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.DETACHED)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += DETACHED_Click;
                    noteFilter.Items.Add(obj);
                }
                if (FileTypesManager.AccountHasPermitionToFile(account.Permition, FileTypesEnum.SHEDULER))
                {
                    obj = new MenuItem
                    {
                        Header = FileTypesManager.Types.Where(q => q.EnumType.Equals(FileTypesEnum.SHEDULER)).FirstOrDefault()?.ToString(),
                        Background = Brushes.LightGray,
                        Foreground = Brushes.Black
                    };
                    obj.Click += SHEDULER_Click;
                    noteFilter.Items.Add(obj);
                }
            }

            page.NoteManagerFilter.Items.Add(noteFilter);
        }

        public void Dispose()
        {
            page.Dispose();
            control.Dispose();
            IsDisposed = true;
            GC.Collect();
        }

        public FileTypesEnum? CheckIfFilterIsSelected()
        {
            foreach (MenuItem element in page.NoteManagerFilter.Items)
            {
                var index = 0;
                var list = new List<Tuple<int, bool, string>>();
                foreach (MenuItem subItem in element.Items)
                {
                    list.Add(new Tuple<int, bool, string>(index, subItem.IsEnabled, subItem.Header.ToString()));
                    index++;
                }

                if (list.Where(q => !q.Item2).Count() > 1)
                {
                    throw new InvalidProgramException("Note Manager list hava invalid enabled items!");
                }

                var item = list.Where(q => !q.Item2).FirstOrDefault();
                if (item == null || item.Item1 == 0)
                    return null;
                else
                {
                    var model = FileTypesManager.SetType(item.Item1 - 1);
                    return model.EnumType;
                }
            }
            return null;
        }

        private void ALL_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.RefreshList();
        }

        private void ACTIVATION_CODE_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.ACTIVATION_CODE);
        }

        private void EXCEPTION_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.EXCEPTION);
        }

        private void INFORMATION_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.INFORMATION);
        }

        private void KEYLOGGER_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.KEYLOGGER);
        }

        private void LOTTO_NOTE_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.LOTTO_NOTE);
        }

        private void NOTE_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.NOTE);
        }

        private void QUERY_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.QUERY);
        }

        private void TASK_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.TASK);
        }

        private void TRACE_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.TRACE);
        }

        private void UNDEFINED_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.UNDEFINED);
        }

        private void DETACHED_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.DETACHED);
        }

        private void SHEDULER_Click(object sender, RoutedEventArgs e)
        {
            control.ClearContentAction();
            SetEditableMenuItem(sender);
            page.InitListView(FileTypesEnum.SHEDULER);
        }

        private void SetEditableMenuItem(object sender)
        {
            foreach (MenuItem item in page.NoteManagerFilter.Items)
            {
                foreach (MenuItem subItem in item.Items)
                {
                    if (subItem.Header.Equals((sender as MenuItem)?.Header))
                        subItem.IsEnabled = false;
                    else
                        subItem.IsEnabled = true;
                }
            }
        }
    }
}
