using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
  internal class SplineBezier : Objeto
  {
    private int ptoSelecionado = 0;
    private char rotuloAtual;
    private readonly int splineQtdPtoMax = 10;
    private int splineQtdPto = 10;

    private Poligono poligono;
    private Ponto[] ptos = new Ponto[4];

    public SplineBezier(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
    {
      rotuloAtual = _rotulo;
      PrimitivaTipo = PrimitiveType.LineStrip;
      PrimitivaTamanho = 1;

      PontosControle();
      PoliedroControle();

      for (int i = 0; i <= splineQtdPto; i++)
        PontosAdicionar(new Ponto4D());

      this.Atualizar();
    }

    public void SplineQtdPto(int inc)
    {
      if (splineQtdPto == splineQtdPtoMax && inc > 0)
        return;

      if (splineQtdPto == 0 && inc < 0)
        return;

      splineQtdPto += inc;
      this.Atualizar();
    }

    public void Atualizar()
    {
      ptos[ptoSelecionado].ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");

      Ponto4D[] controle = new Ponto4D[4] {
        ptos[0].PontosId(0),
        ptos[1].PontosId(0),
        ptos[2].PontosId(0),
        ptos[3].PontosId(0)
      };

      for (int i = 0; i <= splineQtdPto; i++)
      {
        double t = (double)i / splineQtdPto;
        PontosAlterar(CalcularBezier(controle, t), i);
      }

      base.ObjetoAtualizar();
    }

    public void AtualizarSpline(Ponto4D ptoInc, bool proximo)
    {
      ptos[ptoSelecionado].ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      if (proximo)
      {
        ptoSelecionado = (ptoSelecionado + 1) % ptos.Length;
        ptos[ptoSelecionado].ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      }
      else
      {
        ptos[ptoSelecionado].PontosAlterar(ptos[ptoSelecionado].PontosId(0) + ptoInc, 0);
        poligono.PontosAlterar(ptos[ptoSelecionado].PontosId(0), ptoSelecionado);
      }

      Atualizar();
    }

    private void PontosControle()
    {
      ptos[0] = new Ponto(paiRef, ref rotuloAtual, new Ponto4D(-0.5, -0.5));
      ptos[1] = new Ponto(paiRef, ref rotuloAtual, new Ponto4D(-0.5, 0.5));
      ptos[2] = new Ponto(paiRef, ref rotuloAtual, new Ponto4D(0.5, 0.5));
      ptos[3] = new Ponto(paiRef, ref rotuloAtual, new Ponto4D(0.5, -0.5));
    }

    private void PoliedroControle()
    {
      List<Ponto4D> ptosPoligono = new List<Ponto4D>
      {
        ptos[0].PontosId(0),
        ptos[1].PontosId(0),
        ptos[2].PontosId(0),
        ptos[3].PontosId(0)
      };

      Poligono objeto = new(paiRef, ref rotuloAtual, ptosPoligono)
      {
        PrimitivaTipo = PrimitiveType.LineStrip,
        PrimitivaTamanho = 5,
        ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag")
      };

      poligono = objeto;
    }

    private Ponto4D Interpolar(Ponto4D a, Ponto4D b, double t)
    {
      return new Ponto4D(
        a.X + (b.X - a.X) * t,
        a.Y + (b.Y - a.Y) * t
      );
    }

    private Ponto4D CalcularBezier(Ponto4D[] pontos, double t)
    {
      if (pontos.Length == 1)
        return pontos[0];

      Ponto4D[] proximos = new Ponto4D[pontos.Length - 1];

      for (int i = 0; i < proximos.Length; i++)
        proximos[i] = Interpolar(pontos[i], pontos[i + 1], t);

      return CalcularBezier(proximos, t);
    }
  }
}
