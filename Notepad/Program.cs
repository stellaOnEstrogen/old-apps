using System;
using System.IO;
using Terminal.Gui;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Notepad
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fileName = args.Length > 0 ? args[0] : string.Empty;
            bool fileExists = File.Exists(fileName);
            var text = fileExists ? File.ReadAllText(fileName) : string.Empty;

            Application.Init();
            var top = Application.Top;

            string[]? sizeCheck = IsRightSize();
            if (sizeCheck != null)
            {
                MessageBox.ErrorQuery("Error", sizeCheck[0], "Ok");
                return;
            }

            var window = new MainWindow(fileName, text);
            top.Add(window);
            Application.Run();
            
        }

        public static string[]? IsRightSize()
        {
            var frame = Application.Top.Frame;
            if (frame.Width < 80 || frame.Height < 24)
            {
                return new string[] { "The terminal size must be at least 80x24. Please resize your terminal window." };
            }
            return null;
        }
    }

    public class MainWindow : Window
    {
        private TextView _textView;
        private string _fileName;

        public string GetFileName()
        {
            return Path.GetFileName(_fileName);
        }

        public dynamic? FileTypes()
        {
            using var stream = Assembly
                            .GetExecutingAssembly()
                            .GetManifestResourceStream("Notepad.Resources.filetypes.json");

            if (stream == null)
            {
                return null;
            }

            using var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8);

            var content = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject(content);
        }

        public MainWindow(string fileName, string text)
        {
            _fileName = fileName;

            Title = $"Notepad - {(_fileName.Length > 0 ? GetFileName() : "Untitled")}";
            Console.Title = $"Notepad - {(_fileName.Length > 0 ? GetFileName() : "Untitled")}";

            var colorScheme = new ColorScheme
            {
                Normal = Application.Driver.MakeAttribute(Color.Black, Color.Gray), // Normal text
                Focus = Application.Driver.MakeAttribute(Color.White, Color.Blue), // Focus text (e.g., title bar)
                HotNormal = Application.Driver.MakeAttribute(Color.BrightBlue, Color.Gray), // Hot key text
                HotFocus = Application.Driver.MakeAttribute(Color.BrightBlue, Color.Blue) // Hot key focus
            };

            ColorScheme = colorScheme;

            X = 0;
            Y = 1; // Leave one row for the menu bar
            Width = Dim.Fill();
            Height = Dim.Fill();

            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Open", "", OpenFile),
                    new MenuItem("_Save", "", SaveFile),
                    new MenuItem("_New", "", NewFile),
                    new MenuItem("_Quit", "", Quit),
                }),

                new MenuBarItem("_Edit", new MenuItem[]
                {
                    new MenuItem("_Copy", "", () => _textView.Copy()),
                    new MenuItem("_Cut", "", () => _textView.Cut()),
                    new MenuItem("_Paste", "", () => _textView.Paste()),
                }),

                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "", () => MessageBox.Query("About", "This is an application written in .NET 6.0 to mimic an old school Notepad. You can find this project on GitHub at https://github.com/0x7ffed9b08230/old-apps", "Ok")),
                    new MenuItem("_Issues", "", () => MessageBox.Query("Issues", "If you have any issues, please open an issue by visiting https://github.com/0x7ffed9b08230/old-apps/issues", "Ok")),
                    new MenuItem("_License", "", () => MessageBox.Query("License", "This project is licensed under the CC0 1.0 Universal license. You can find the full license text at https://creativecommons.org/publicdomain/zero/1.0/legalcode", "Ok"))
                }),

                new MenuBarItem("_Exit", new MenuItem[]
                {
                    new MenuItem("_Exit", "", Quit)
                })
            });

            _textView = new TextView
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Text = text
            };

            Add(_textView);
            Application.Top.Add(menu);
        }

        private void OpenFile()
        {
            var openDialog = new OpenDialog("Open", "Open a file")
            {
                AllowsMultipleSelection = false,
                CanChooseDirectories = false
            };

            Application.Run(openDialog);

            if (!openDialog.Canceled)
            {
                _fileName = openDialog.FilePath?.ToString() ?? string.Empty;
                _textView.Text = File.ReadAllText(_fileName);
                Title = $"Notepad - {GetFileName()}";
            }
        }

        private void NewFile()
        {
            var fileTypes = FileTypes();

            if (fileTypes == null)
            {
                MessageBox.ErrorQuery("Error", "Could not load file types.", "Ok");
                return;
            }

            var fileTypeItems = new List<string>();

            foreach (var fileType in fileTypes.filetypes)
            {
                fileTypeItems.Add(fileType.name.ToString());
            }

            var fileTypeListDialog = new Dialog("Select File Type", 50, 20);

            var fileTypeListView = new ListView(fileTypeItems)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            var cancelButton = new Button("Cancel")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(fileTypeListView) + 1
            };

            cancelButton.Clicked += () => fileTypeListDialog.Running = false;

            fileTypeListDialog.Add(fileTypeListView);
            fileTypeListDialog.Add(cancelButton);
            Application.Run(fileTypeListDialog);

            if (fileTypeListView.SelectedItem == -1)
            {
                return;
            }

            var selectedFileType = fileTypes.filetypes[fileTypeListView.SelectedItem];
            var extensions = ((JArray)selectedFileType.extensions).ToObject<string[]>();

            string selectedExtension = extensions[0];

            if (extensions.Length > 1)
            {
                var extensionListDialog = new Dialog("Select Extension", 50, 20);

                var extensionListView = new ListView(extensions)
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };

                extensionListDialog.Add(extensionListView);
                Application.Run(extensionListDialog);

                if (extensionListView.SelectedItem == -1)
                {
                    return;
                }

                selectedExtension = extensions[extensionListView.SelectedItem];
            }

            // Name the file but force the extension
            var saveDialog = new SaveDialog("Save As", "Save file as")
            {
                FilePath = _fileName,
            };

            Application.Run(saveDialog);

            if (!saveDialog.Canceled)
            {
                _fileName = saveDialog.FilePath?.ToString() ?? string.Empty;
                File.WriteAllText($"{_fileName}.{selectedExtension}", string.Empty);
                Title = $"Notepad - {GetFileName()}";
            }


        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                var saveDialog = new SaveDialog("Save As", "Save file as")
                {
                    FilePath = _fileName
                };

                Application.Run(saveDialog);

                if (!saveDialog.Canceled)
                {
                    _fileName = saveDialog.FilePath?.ToString() ?? string.Empty;
                    File.WriteAllText(_fileName, _textView.Text.ToString());
                    Title = $"Notepad - {GetFileName()}";
                }
            }

            if (!string.IsNullOrEmpty(_fileName))
            {
                File.WriteAllText(_fileName, _textView.Text.ToString());
            }
        }

        private void Quit()
        {
            Application.RequestStop();
        }
    }
}
