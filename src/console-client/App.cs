using Terminal.Gui;
using NStack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

Application.Init();
var top = Application.Top;

// Creates the top-level window to show
var win = new Window("MyApp")
{
	X = 0,
	Y = 1, // Leave one row for the toplevel menu

	// By using Dim.Fill(), it will automatically resize without manual intervention
	Width = Dim.Fill(),
	Height = Dim.Fill()
};

top.Add(win);

// Creates a menubar, the item "New" has a help menu.
var menu = new MenuBar(new MenuBarItem[] {
			new MenuBarItem ("_File", new MenuItem [] {
				new MenuItem ("_New", "Creates new file", null),
				new MenuItem ("_Close", "",null),
				new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
			}),
			new MenuBarItem ("_Edit", new MenuItem [] {
				new MenuItem ("_Copy", "", null),
				new MenuItem ("C_ut", "", null),
				new MenuItem ("_Paste", "", null)
			})
		});
top.Add(menu);

static bool Quit()
{
	var n = MessageBox.Query(50, 7, "Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");
	return n == 0;
}

var login = new Label("Login: ") { X = 3, Y = 2 };
var password = new Label("Password: ")
{
	X = Pos.Left(login),
	Y = Pos.Top(login) + 1
};
var loginText = new TextField("")
{
	X = Pos.Right(password),
	Y = Pos.Top(login),
	Width = 40
};
var passText = new TextField("")
{
	Secret = true,
	X = Pos.Left(loginText),
	Y = Pos.Top(password),
	Width = Dim.Width(loginText)
};

var serilogLogger = new LoggerConfiguration()
           .WriteTo.File("output.txt")
           .CreateLogger();

var serviceProvider = new ServiceCollection()
	.AddLogging(builder =>
		{
			builder.SetMinimumLevel(LogLevel.Information);
			builder.AddSerilog(logger: serilogLogger, dispose: true);
		})
	.AddSingleton<ICmmrcClient, CmmrcClient>()
	.BuildServiceProvider();

var logger = serviceProvider
	.GetService<ILoggerFactory>()
	.CreateLogger<Program>();
logger.LogInformation("Logging from Program.cs");

var ok = new Button(3, 14, "Ok");
ok.Clicked += async () => {
	var tunnel = serviceProvider.GetService<ICmmrcClient>();
	var result = await tunnel.TestConnection();
	MessageBox.Query(50, 7, "Result", result, "Ok");
};

// Add some controls, 
win.Add(
	// The ones with my favorite layout system, Computed
	login, password, loginText, passText,

	// The ones laid out like an australopithecus, with Absolute positions:
	new CheckBox(3, 6, "Remember me"),
	new RadioGroup(3, 8, new ustring[] { "_Personal", "_Company" }, 0),
    ok,
	new Button(10, 14, "Cancel"),
	new Label(3, 18, "Press F9 or ESC plus 9 to activate the menubar")
);

Application.Run();
Application.Shutdown();