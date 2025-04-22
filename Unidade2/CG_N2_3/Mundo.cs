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
    private static Objeto mundo = null;

    private char rotuloAtual = '?';
    private Dictionary<char, Objeto> grafoLista = [];
    private Objeto objetoSelecionado = null;
    private Transformacao4D matrizGrafo = new();

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

    private bool mouseMovtoPrimeiro = true;
    private Ponto4D mouseMovtoUltimo;

    private Retangulo meuRetangulo;

    private Circulo meuCirculo;

    private SegReta meuSegmento;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
      : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloAtual); //padrão Singleton
    }

    protected override void OnLoad()
    {
      base.OnLoad();

      //meuRetangulo = new Retangulo(mundo, ref rotuloAtual);
      //meuCirculo = new Circulo(mundo, ref rotuloAtual);
      meuSegmento = new SegReta(mundo, ref rotuloAtual);

      Utilitario.Diretivas();
#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      #region Cores
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      #endregion


    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit);

      matrizGrafo.AtribuirIdentidade();
      mundo.Desenhar(matrizGrafo, objetoSelecionado);
      //mundo.Desenhar(matrizGrafo, meuRetangulo);
      //mundo.Desenhar(matrizGrafo, meuCirculo);
      mundo.Desenhar(matrizGrafo, objetoSelecionado);

      SwapBuffers();
    }

    int posAux = 0;
    double x = 0;
    double y = 0;
    double ySegundo = 0;
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      var estadoTeclado = KeyboardState;
      if (estadoTeclado.IsKeyPressed(Keys.Space))
      {
        objetoSelecionado = Grafocena.GrafoCenaProximo(mundo, objetoSelecionado, grafoLista);
      }
      if (estadoTeclado.IsKeyPressed(Keys.Q))
      {
        double x1 = objetoSelecionado.PontosId(0).X;
        double y1 = objetoSelecionado.PontosId(0).Y;
        double x2 = objetoSelecionado.PontosId(1).X;
        double y2 = objetoSelecionado.PontosId(1).Y;

        objetoSelecionado.PontosAlterar(new Ponto4D(x1 - 0.05, y1, 0), 0);
        objetoSelecionado.PontosAlterar(new Ponto4D(x2 - 0.05, y2, 0), 1);
        objetoSelecionado.ObjetoAtualizar();

      }
      if (estadoTeclado.IsKeyPressed(Keys.W))
      {
        double x1 = objetoSelecionado.PontosId(0).X;
        double y1 = objetoSelecionado.PontosId(0).Y;
        double x2 = objetoSelecionado.PontosId(1).X;
        double y2 = objetoSelecionado.PontosId(1).Y;

        objetoSelecionado.PontosAlterar(new Ponto4D(x1 + 0.05, y1, 0), 0);
        objetoSelecionado.PontosAlterar(new Ponto4D(x2 + 0.05, y2, 0), 1);
        objetoSelecionado.ObjetoAtualizar();
      }

      if (estadoTeclado.IsKeyPressed(Keys.A))
      {

        double x = objetoSelecionado.PontosId(1).X;
        double y = objetoSelecionado.PontosId(1).Y;

        objetoSelecionado.PontosAlterar(new Ponto4D(x - 0.05, y - 0.05, 0), 1);
        objetoSelecionado.ObjetoAtualizar();
      }
      if (estadoTeclado.IsKeyPressed(Keys.S))
      {
        double x = objetoSelecionado.PontosId(1).X;
        double y = objetoSelecionado.PontosId(1).Y;

        objetoSelecionado.PontosAlterar(new Ponto4D(x + 0.05, y + 0.05, 0), 1);
        objetoSelecionado.ObjetoAtualizar();
        objetoSelecionado.ObjetoAtualizar();
      }
      if (estadoTeclado.IsKeyPressed(Keys.X))
      {
        var p0 = objetoSelecionado.PontosId(0);
        var p1 = objetoSelecionado.PontosId(1);

        double centroX = (p0.X + p1.X) / 2;
        double centroY = (p0.Y + p1.Y) / 2;
   
        objetoSelecionado.PontosAlterar(Rotacionar(centroX, centroY,p0,'H'), 0);
        objetoSelecionado.PontosAlterar(Rotacionar(centroX, centroY,p1,'H'), 1);
        objetoSelecionado.ObjetoAtualizar();
      }
      if(estadoTeclado.IsKeyPressed(Keys.Z)){
        var p0 = objetoSelecionado.PontosId(0);
        var p1 = objetoSelecionado.PontosId(1);

        double centroX = (p0.X + p1.X) / 2;
        double centroY = (p0.Y + p1.Y) / 2;

        objetoSelecionado.PontosAlterar(Rotacionar(centroX, centroY, p0,'A'), 0);
        objetoSelecionado.PontosAlterar(Rotacionar(centroX, centroY, p1,'A'), 1);
        objetoSelecionado.ObjetoAtualizar();
      }
    }

    private Ponto4D Rotacionar(double centroX, double centroY, Ponto4D p, char sentido){
          double angulo;
          if(sentido == 'H'){
            angulo = MathHelper.DegreesToRadians(-5);
          }else{
            angulo = MathHelper.DegreesToRadians(5);
          }
          
          double x = p.X;
          double y = p.Y;
          double xNovo = Math.Cos(angulo) * (x - centroX) - Math.Sin(angulo) * (y - centroY) + centroX;
          double yNovo = Math.Sin(angulo) * (x - centroX) + Math.Cos(angulo) * (y - centroY) + centroY;
          return new Ponto4D(xNovo, yNovo, 0);
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


      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);

      base.OnUnload();
    }



  }
}
