# v1.23
* Fixes a bug with the allowed/denied extensions. There are no breaking changes

# v1.22
* Re-release. Problem with the marketplace.

# v1.21
* Re-release. Problem with the marketplace.

# v1.20
* Maintenance. Adds async package initialization, 10x [yhvicey](https://github.com/yhvicey) for reporting this issue

# v1.19
* Adds support for VS2019, 10x [Profi-Concept](https://github.com/Profi-Concept)

# v1.18
* Fixes a bug with the allowed/denied extension settings

# v1.17
* Ability to configure the extension via a .formatconfig file. The benefit is that you can have settings per repository.

# v1.16
* [fyst](https://github.com/fyst): Fix FormatDocumentOnSavePackage from not working under various Visual Studio configuration (ex. Folder View).

# v1.15
* [Schobers](https://github.com/Schobers) added a configuration option to execute another command on save (it defaults to "Edit.FormatDocument" if not filled in). (see [http://llvm.org/builds/](http://llvm.org/builds/); commands: Tools.ClangFormatDocument)

# v1.14
* Added support for Visual Studio 2017 (RC). If this breaks your Visual Studio I will revert it.

# v1.13
* Make it possible to install on all systems which have at least .NET 4.5

# v1.12
* Fix bug when multiple file extensions are denied

# v1.11
* Fix Visual Studio 2013 support once and for ever!

# v1.10
* Add tools/options page where you can configure which files will be formatted

# v1.9
* Fixed a problem with Visual Studio 2013

# v1.8
* Added support for Visual Studio 2015 CTP6\. If this breaks your Visual Studio 2013 I will revert it.

# v1.7
* Format files when launching unit test from Resharper. 10x manuc66

# v1.6
* Fix bug in *.cshtml that the view jumps around

# v1.5
* Improve user experience and performance

# v1.4
* Do not show error message if the document does not support formatting

# v1.3
* Version bump

# v1.2
* All text files are supported

# v1.1
* Add support for SaveAll