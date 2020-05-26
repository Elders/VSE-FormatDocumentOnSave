VSE-FormatDocumentOnSave
========================
Enables auto formatting of the code when you save a file. Visual Studio supports auto formatting of the code with the CTRL+E,D or CTRL+E,F key shortcuts but with this extension the command `Edit.FormatDocument` is executed on Save.

# Download
https://marketplace.visualstudio.com/items?itemName=mynkow.FormatdocumentonSave

# Configuration
There are 3 settings which you could configure:
* Command
* Allowed extensions
* Denied Extensions
* Enable in Debug

### Command
This is the Visual Studio command which will be invoked when a document is saved. For multiple values you could use `space` separated list.
##### Default
`Edit.FormatDocument`

### Allowed extensions
Specifies all file extensions where the `command` is allowed to be executed. For multiple values you could use `space` separated list.
##### Default
`allowed_extensions = .*`

### Denied extensions
Specifies all file extensions where the `command` is NOT allowed to be executed. For multiple values you could use `space` separated list.
##### Default
`denied_extensions = `

### Debug mode
By default the plugin is disabled in debug mode. You could explicitly configure to have the extension enabled while in a debug session.
##### Default
`enable_in_debug = false`

# Examples
## Scenario 1
- `allowed_extensions = .*`
- `denied_extensions = .cs` 

**Result:** All documents will be formatted because we explicitly specified that all extensions will be formatted using `allowed_extensions = .*`.

## Scenario 2
- `allowed_extensions = `
- `denied_extensions = .js` 

**Result:** All documents will be formatted except those with `.js` extension

## Scenario 3
- `allowed_extensions = `
- `denied_extensions = .*` 

**Result:** No documents will be formatted

## Scenario 4
- `allowed_extensions = .cs`
- `denied_extensions = .*` 

**Result:** Only documents with `.cs` extension will be formatted

## Scenario 5
- `allowed_extensions = .cs`
- `denied_extensions =` 

**Result:** All documents will be formatted because nothing is denied

## Scenario 6
- `allowed_extensions = .cs`
- `denied_extensions = .cs` 

**Result:** All documents will be formatted because there is a conflict 

# Visual Studio
You can configure these settings from the Visual Studio Options menu

# Format Config

Create a `.formatconfig` file in the root of your project 

Note: If you have a `.formatconfig` file, the VS options are ignored!

## Example
```
root = true

[*.*]
command = Edit.FormatDocument
allowed_extensions = 
denied_extensions = .js .html
```

## Example with multiple commands
```
root = true

[*.*]

command = Edit.FormatDocument Edit.FormatDocument
allowed_extensions = 
denied_extensions = .js .html
```

# Disable the extension
If you wish to temporarily disable to extension you could press CapsLock.  
By default the extension is disabled while in a debug session. You could change that from the configuration.
