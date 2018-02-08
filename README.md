VSE-FormatDocumentOnSave
========================
Enables auto formatting of the code when you save a file. Visual Studio supports auto formatting of the code with the CTRL+E,D or CTRL+E,F key shortcuts but with this extension the command 'Format Document' is executed on Save.

# Configuration
There are 3 settings which you could configure

## Command
This is the Visual Studio command which will be invoked when you press `ctrl + s`.
### Default
`Edit.FormatDocument`
### Example
`command = Edit.FormatDocument`

### Allowed extensions
Specifies all file extensions where the `command` is allowed to be executed. For multiple values you could use `space` separated list.
### Default
`allowed_extensions = .*`
### Example
allowed_extensions = .*

### Denied extensions
Specifies all file extensions where the `command` is NOT allowed to be executed. For multiple values you could use `space` separated list.
### Default
`denied_extensions = `
### Example
`denied_extensions = .js .html`


## Visual Studio
You can configure these settings from the Visual Studio Options menu

## Format Config

`.formatconfig`
```
root = true

[*.*]
command = Edit.FormatDocument
allowed_extensions = .*
denied_extensions = .js .html
```