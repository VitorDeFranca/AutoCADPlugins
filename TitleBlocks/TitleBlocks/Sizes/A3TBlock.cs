using TitleBlocks.Model;

namespace TitleBlocks.Sizes
{
    public class A3TBlock : TBlock
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public A3TBlock()
        {
            Height = 297;
            Width = 420;
        }
    }
}
