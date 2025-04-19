using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class MyBBox : Objeto
  {
    private double menorX, menorY, maiorX, maiorY;
    public MyBBox(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, new Ponto4D(-0.5, -0.5), new Ponto4D(0.5, 0.5))
    {

    }

    public MyBBox(Objeto _paiRef, ref char _rotulo, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.Points;
      PrimitivaTamanho = 10;

      // Sentido horário
      base.PontosAdicionar(ptoInfEsq);
      base.PontosAdicionar(new Ponto4D(ptoSupDir.X, ptoInfEsq.Y));
      base.PontosAdicionar(ptoSupDir);
      base.PontosAdicionar(new Ponto4D(ptoInfEsq.X, ptoSupDir.Y));
      Atualizar();
    }

    public bool Dentro(Ponto4D pto)
    {
      return (
        pto.X >= ObterMenorX && pto.X <= ObterMaiorX &&
        pto.Y >= ObterMenorY && pto.Y <= ObterMaiorY
      );
    }

    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public double ObterMenorX => menorX;
    public double ObterMenorY => menorY;
    public double ObterMaiorX => maiorX;
    public double ObterMaiorY => maiorY;
  }
}
