using Game_Tools_Week4_Editor;
using System.Threading;
using Game_Tools_Week4_Editor;
using Game_Tools_Week4_Editor.Editor;


// Set STA thread mode for OpenFileDialog to work
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

FormEditor editor = new();
editor.Game = new GameEditor(editor);
editor.Show();
editor.Game.Run();
