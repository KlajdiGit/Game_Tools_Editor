using Game_Tools_Week4_Editor;
using System.Threading;
using Game_Tools_Week4_Editor.Editor;


// Set STA thread mode for OpenFileDialog to work
Thread t = Thread.CurrentThread;
t.SetApartmentState(ApartmentState.Unknown);
t.SetApartmentState(ApartmentState.STA);


FormEditor editor = new();
editor.Game = new GameEditor(editor);
editor.Show();
editor.Game.Run();
