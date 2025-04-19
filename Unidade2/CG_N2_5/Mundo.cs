/*
 As constantes dos pré-processors estão nos arquivos ".csproj"
 desse projeto e da CG_Biblioteca.
*/

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace gcgcg
{
  public class Mundo : GameWindow
  {

#if CG_Gizmo
    private readonly float[] _sruEixos =
    [
       0.0f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f,  0.0f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f,  0.0f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    ];
    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    // FPS
    private int frames = 0;
    private Stopwatch stopwatch = new();
#endif

    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;

    private static Objeto mundo = null;
    private char rotuloAtual = '?';
    private Dictionary<char, Objeto> grafoLista = [];
    private Objeto objetoSelecionado = null;
    private Transformacao4D matrizGrafo = new();
    private bool mouseMovtoPrimeiro = true;
    private Ponto4D mouseMovtoUltimo;

    // Adicionado custom
    private Retangulo limitBox;
    private Circulo circuloMaior;
    private Retangulo myBBox;
    private Circulo circuloMenor;
    private Ponto centroCirculo;
    private double distanciaDeslocamento = 0.01;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloAtual); // Padrão Singleton
    }

    protected override void OnLoad()
    {
      base.OnLoad();
      Utilitario.Diretivas();
      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");

#if CG_Gizmo
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);

      stopwatch.Start();
#endif


      #region CG_N2_5
      limitBox = new Retangulo(mundo, ref rotuloAtual, new Ponto4D(0, 0), new Ponto4D(0.6, 0.6));
      circuloMaior = new Circulo(mundo, ref rotuloAtual, 0.3f, limitBox.ObterCentro);
      myBBox = new Retangulo(mundo, ref rotuloAtual, circuloMaior.getPontoAngulo(225), circuloMaior.getPontoAngulo(45));
      circuloMenor = new Circulo(mundo, ref rotuloAtual, 0.1f, myBBox.ObterCentro);
      centroCirculo = new Ponto(circuloMenor, ref rotuloAtual, circuloMenor.ptoDeslocamento)
      {
        PrimitivaTamanho = 10
      };

      objetoSelecionado = limitBox;
      #endregion


    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);


      #region Teclado
      var estadoTeclado = KeyboardState;
      if (estadoTeclado.IsKeyPressed(Keys.Escape))
        Close();

      if ((estadoTeclado.IsKeyPressed(Keys.Up) || estadoTeclado.IsKeyPressed(Keys.C)))
      {
        MoverJoystick(new Ponto4D(circuloMenor.ptoDeslocamento.X, circuloMenor.ptoDeslocamento.Y + distanciaDeslocamento));
      }

      if ((estadoTeclado.IsKeyPressed(Keys.Down) || estadoTeclado.IsKeyPressed(Keys.B)))
      {
        MoverJoystick(new Ponto4D(circuloMenor.ptoDeslocamento.X, circuloMenor.ptoDeslocamento.Y - distanciaDeslocamento));
      }

      if ((estadoTeclado.IsKeyPressed(Keys.Right) || estadoTeclado.IsKeyPressed(Keys.D)))
      {
        MoverJoystick(new Ponto4D(circuloMenor.ptoDeslocamento.X + distanciaDeslocamento, circuloMenor.ptoDeslocamento.Y));
      }

      if ((estadoTeclado.IsKeyPressed(Keys.Left) || estadoTeclado.IsKeyPressed(Keys.E)))
      {
        MoverJoystick(new Ponto4D(circuloMenor.ptoDeslocamento.X - distanciaDeslocamento, circuloMenor.ptoDeslocamento.Y));
      }
      #endregion


      #region  Mouse
      int janelaLargura = ClientSize.X;
      int janelaAltura = ClientSize.Y;
      Ponto4D mousePonto = new(MousePosition.X, MousePosition.Y);
      Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

      if (estadoTeclado.IsKeyPressed(Keys.LeftShift))
      {
        if (mouseMovtoPrimeiro)
        {
          mouseMovtoUltimo = sruPonto;
          mouseMovtoPrimeiro = false;
        }
        else
        {
          var deltaX = sruPonto.X - mouseMovtoUltimo.X;
          var deltaY = sruPonto.Y - mouseMovtoUltimo.Y;
          mouseMovtoUltimo = sruPonto;

          objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(0).X + deltaX, objetoSelecionado.PontosId(0).Y + deltaY, 0), 0);
          objetoSelecionado.ObjetoAtualizar();
        }
      }
      if (estadoTeclado.IsKeyPressed(Keys.LeftShift))
      {
        objetoSelecionado.PontosAlterar(sruPonto, 0);
        objetoSelecionado.ObjetoAtualizar();
      }

      #endregion


    }

    private void MoverJoystick(Ponto4D ptoDeslocamento)
    {
      // Ponto original
      double x = circuloMenor.ptoDeslocamento.X;
      double y = circuloMenor.ptoDeslocamento.Y;

      double centro_x = circuloMaior.ptoDeslocamento.X;
      double centro_y = circuloMaior.ptoDeslocamento.Y;

      double dx = ptoDeslocamento.X - centro_x;
      double dy = ptoDeslocamento.Y - centro_y;

      // Elevado ao quadrado
      double distancia = (dx * dx) + (dy * dy);
      double raio = circuloMaior.raio * circuloMaior.raio;

      if (distancia < raio)
      {
        x = ptoDeslocamento.X;
        y = ptoDeslocamento.Y;
      }

      // primitiva BBox
      if (ptoDeslocamento.X < myBBox.ObterMenorX || ptoDeslocamento.X > myBBox.ObterMaiorX ||
          ptoDeslocamento.Y < myBBox.ObterMenorY || ptoDeslocamento.Y > myBBox.ObterMaiorY)
        myBBox.PrimitivaTipo = PrimitiveType.Points;
      else
        myBBox.PrimitivaTipo = PrimitiveType.LineLoop;

      circuloMenor.Atualizar(new Ponto4D(x, y));
      centroCirculo.PontosAlterar(new Ponto4D(x, y), 0);
      centroCirculo.ObjetoAtualizar();
    }


    // =============================================== //


    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit);

      matrizGrafo.AtribuirIdentidade();
      mundo.Desenhar(matrizGrafo, objetoSelecionado);

#if CG_Gizmo
      Gizmo_Sru3D();

      frames++;
      if (stopwatch.ElapsedMilliseconds >= 1000)
      {
        Console.WriteLine($"FPS: {frames}");
        frames = 0; 
        stopwatch.Restart();
      }
#endif
      SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);
      GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }

    protected override void OnUnload()
    {
      mundo.OnUnload();

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

#if CG_Gizmo
      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);
#endif

      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);

      base.OnUnload();
    }

    private void Gizmo_Sru3D()
    {
#if CG_Gizmo
#if CG_OpenGL
      var transform = Matrix4.Identity;
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.SetMatrix4("transform", transform);
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.SetMatrix4("transform", transform);
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.SetMatrix4("transform", transform);
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#endif
#endif
    }

  }
}
