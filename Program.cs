using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace OpenGLRectangle
{
    public class RectangleWindow : GameWindow
    {
        private int vertexBufferObject;
        private int vertexArrayObject;
        private int shaderProgram;

        // Vertex data for a rectangle (two triangles)
        private readonly float[] vertices = {
            // Position (X, Y, Z)    // Color (R, G, B)
            -0.5f,  0.5f, 0.0f,     1.0f, 0.0f, 0.0f,  // Top-left (red)
             0.5f,  0.5f, 0.0f,     0.0f, 1.0f, 0.0f,  // Top-right (green)
             0.5f, -0.5f, 0.0f,     0.0f, 0.0f, 1.0f,  // Bottom-right (blue)
            
            -0.5f,  0.5f, 0.0f,     1.0f, 0.0f, 0.0f,  // Top-left (red)
             0.5f, -0.5f, 0.0f,     0.0f, 0.0f, 1.0f,  // Bottom-right (blue)
            -0.5f, -0.5f, 0.0f,     1.0f, 1.0f, 0.0f   // Bottom-left (yellow)
        };

        // Vertex shader source code
        private const string vertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec3 aColor;
            
            out vec3 vertexColor;
            
            void main()
            {
                gl_Position = vec4(aPosition, 1.0);
                vertexColor = aColor;
            }
        ";

        // Fragment shader source code
        private const string fragmentShaderSource = @"
            #version 330 core
            in vec3 vertexColor;
            out vec4 FragColor;
            
            void main()
            {
                FragColor = vec4(vertexColor, 1.0);
            }
        ";

        public RectangleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Set the background color
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Create and bind Vertex Array Object
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // Create and bind Vertex Buffer Object
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Create and compile shaders
            shaderProgram = CreateShaderProgram();

            // Configure vertex attributes
            // Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Color attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Unbind VAO
            GL.BindVertexArray(0);
        }

        private int CreateShaderProgram()
        {
            // Create vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            // Check vertex shader compilation
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexStatus);
            if (vertexStatus != 1)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                throw new Exception($"Vertex shader compilation failed: {infoLog}");
            }

            // Create fragment shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            // Check fragment shader compilation
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentStatus);
            if (fragmentStatus != 1)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                throw new Exception($"Fragment shader compilation failed: {infoLog}");
            }

            // Create shader program and link shaders
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            // Check program linking
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int programStatus);
            if (programStatus != 1)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Shader program linking failed: {infoLog}");
            }

            // Clean up individual shaders (they're linked to the program now)
            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Use our shader program
            GL.UseProgram(shaderProgram);

            // Bind the VAO and draw the rectangle
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            // Swap buffers
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Clean up resources
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
            GL.DeleteProgram(shaderProgram);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Close window on Escape key
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Configure window settings
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "OpenGL Rectangle Renderer",
            };

            var gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.UpdateFrequency = 60.0;

            // Create and run the window
            using (var window = new RectangleWindow(gameWindowSettings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}