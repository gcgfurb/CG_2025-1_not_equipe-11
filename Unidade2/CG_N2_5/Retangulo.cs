using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Retangulo : Objeto
  {
    private double menorX, menorY, maiorX, maiorY;
    private readonly Ponto4D centro = new();

    public Retangulo(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, new Ponto4D(-0.5, -0.5), new Ponto4D(0.5, 0.5))
    {

    }

    public Retangulo(Objeto _paiRef, ref char _rotulo, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.LineLoop;
      PrimitivaTamanho = 10;

      GerarPontos(ptoInfEsq, ptoSupDir);
      Atualizar();
    }

    private void GerarPontos(Ponto4D ptoInfEsq, Ponto4D ptoSupDir)
    {
      menorX = ptoInfEsq.X;
      menorY = ptoInfEsq.Y;
      maiorX = ptoSupDir.X;
      maiorY = ptoSupDir.Y;
      centro.X = (maiorX + menorX) / 2;
      centro.Y = (maiorY + menorY) / 2;

      // Sentido anti-horário
      base.PontosAdicionar(new Ponto4D(menorX, menorY));
      base.PontosAdicionar(new Ponto4D(maiorX, menorY));
      base.PontosAdicionar(new Ponto4D(maiorX, maiorY));
      base.PontosAdicionar(new Ponto4D(menorX, maiorY));
    }

    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public bool Dentro(Ponto4D pto)
    {
      return (
          pto.X >= ObterMenorX && pto.X <= ObterMaiorX &&
          pto.Y >= ObterMenorY && pto.Y <= ObterMaiorY
      );
    }

    public double ObterMenorX => menorX;
    public double ObterMenorY => menorY;
    public double ObterMaiorX => maiorX;
    public double ObterMaiorY => maiorY;
    public Ponto4D ObterCentro => centro;
  }
}
