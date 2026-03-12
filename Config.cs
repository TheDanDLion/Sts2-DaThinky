using BaseLib.Config;

namespace DaThinky;

public class Config : SimpleModConfig
{
	// Properties must be public static with both getters and setters. SimpleModConfig currently only auto-generates
	// UI for bool types (as tickboxes).
	public static bool ModOption { get; set; } = true;
}
