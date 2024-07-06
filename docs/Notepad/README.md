# Notepad Application Documentation

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Installation](#installation)
4. [Usage](#usage)
   - [Launching the Application](#launching-the-application)
   - [Main Window](#main-window)
   - [Menu Options](#menu-options)
5. [File Operations](#file-operations)
   - [Open a File](#open-a-file)
   - [Save a File](#save-a-file)
   - [New File](#new-file)
   - [Quit the Application](#quit-the-application)
6. [Editing Operations](#editing-operations)
   - [Copy](#copy)
   - [Cut](#cut)
   - [Paste](#paste)
7. [Help Options](#help-options)
   - [About](#about)
   - [Issues](#issues)
   - [License](#license)
8. [Exit](#exit)
9. [Dependencies](#dependencies)

## Introduction

The Notepad application is a simple text editor written in C# using the .NET 6.0 framework. It mimics the functionality of an old-school notepad application, providing basic text editing features.

## Features

- Open, create, and save text files.
- Basic text editing (copy, cut, paste).
- User-friendly terminal interface.
- Customizable file types for new files.
- Help menu with information about the application and licensing.

## Installation

To install and run the Notepad application, follow these steps:

1. Ensure you have .NET 6.0 SDK installed on your machine.

2. Clone the repository from GitHub:
   ```sh
   git clone https://github.com/0x7ffed9b08230/old-apps
   ```
3. Navigate to the Notepad application directory:
   ```sh
   cd old-apps/Notepad
   ```

4. Build the application using the .NET CLI:
   ```sh
    dotnet build
    ```

5. Run the application:
    ```sh
    dotnet run
    ```

## Usage

### Launching the Application

You can launch the application with or without a file argument. If you provide a file name, the application will open that file. If no file name is provided, the application will start with an empty editor.

```sh
dotnet run [optional-file-name]
```

### Main Window

Upon launching, the main window of the Notepad application will be displayed. It includes a menu bar and a text editing area.

### Menu Options

The menu bar provides various options for file operations, editing, and help.


### File Operations

#### Open a File

To open an existing file:

1. Select `File` > `Open` from the menu.

2. A dialog will appear to choose a file.

3. Select the desired file and press OK.

#### Save a File

To create a new file:

1. Select `File` > `Save` from the menu.
2. If the file has no name, a save dialog will appear.
3. Enter the file name and press OK.

#### New File

To create a new file:

1.  Select `File` > `New` from the menu.
2.  Choose a file type from the displayed list.
3.  Select an extension (if multiple are available).
4.  A save dialog will appear to name the new file.

#### Quit the Application

To exit the application:

1. Select `File` > `Quit` or `Exit` > `Exit` from the menu.

### Editing Operations

#### Copy

To copy text:

1. Select the text you want to copy.
2. Right-click and select `Copy` from the context menu. Alternatively, Select `Edit` > `Copy` from the menu.

#### Cut

To cut text:

1. Select the text you want to cut.
2. Right-click and select `Cut` from the context menu. Alternatively, Select `Edit` > `Cut` from the menu.

#### Paste

To paste text:

1. Right-click and select `Paste` from the context menu. Alternatively, Select `Edit` > `Paste` from the menu.

### Help Options

#### About

To view information about the application:

1. Select `Help` > `About` from the menu.

#### Issues

To report issues or bugs:

1. Select `Help` > `Issues` from the menu.

#### License

To view the application's license:

1. Select `Help` > `License` from the menu.

## Exit

To exit the application:

1. Select `File` > `Quit` or `Exit` > `Exit` from the menu.

## Dependencies

The Notepad application has the following dependencies:
- [Newtonsoft.Json](https://www.newtonsoft.com/json) - for JSON serialization and deserialization.