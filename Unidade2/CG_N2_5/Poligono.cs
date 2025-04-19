using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Poligono : Objeto
  {
    public Poligono(Objeto _paiRef, ref char _rotulo, List<Ponto4D> pontosPoligono) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.LineLoop;
      PrimitivaTamanho = 1;
      base.pontosLista = pontosPoligono;
      Atualizar();
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }
  }
}
