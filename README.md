# OpenGL Rectangle Renderer

A simple OpenGL application written in C# using OpenTK that renders a colorful 2D rectangle.
<img width="1142" height="791" alt="image" src="https://github.com/user-attachments/assets/2d0ea3a4-1197-421c-ae53-53ef1a9fc19d" />


## Features

- Creates an OpenGL-compatible window (800x600)
- Renders a single rectangle with gradient colors
- Uses modern OpenGL with shaders (OpenGL 3.3 Core Profile)
- Proper resource cleanup on exit
- ESC key to close the window

## Prerequisites

- Visual Studio 2022
- .NET 6.0 SDK or later
- Windows, macOS, or Linux

## Setup Instructions

### Method 1: Using Visual Studio

1. Clone this repository:
   ```bash
   git clone https://github.com/FurqanKhurrum/OpenGLRectangle.git
   ```

2. Open the solution in Visual Studio

3. Build the solution (Ctrl+Shift+B)

4. Run the project (F5)

### Method 2: Using Command Line

1. Clone this repository:
   ```bash
   git clone https://github.com/FurqanKhurrum/OpenGLRectangle.git
   ```

2. Navigate to the project directory:
   ```bash
   cd OpenGLRectangle
   ```

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Build the project:
   ```bash
   dotnet build
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

## Project Structure

```
OpenGLRectangle/
├── Program.cs              # Main application code
├── OpenGLRectangle.csproj  # Project configuration
├── README.md              # This file
└── .gitignore            # Git ignore file
```

## How It Works

1. **Window Creation**: The application creates an 800x600 window using OpenTK's GameWindow class

2. **Shader Setup**: Vertex and fragment shaders are compiled to handle vertex positions and colors

3. **Rectangle Data**: The rectangle is defined as two triangles with position and color data for each vertex

4. **Rendering Loop**: The application runs at 60 FPS, continuously rendering the rectangle

5. **Color Gradient**: Each corner of the rectangle has a different color, creating a gradient effect

## Controls

- **ESC**: Close the application

## Technologies Used

- **C#** - Programming language
- **OpenTK 4.9.4** - .NET wrapper for OpenGL (SharpDX is no longer under active development or maintenance)
- **OpenGL 3.3** - Graphics API
- **.NET 6.0** - Framework

## Author

Furqan Khurrum

## Acknowledgments

- Assignment for Game Engine Foundations course
- Based on OpenTK tutorials and documentation
