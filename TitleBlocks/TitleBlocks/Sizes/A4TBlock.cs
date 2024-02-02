using TitleBlocks.Model;

namespace TitleBlocks.Sizes
{
    public class A4TBlock : TBlock
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public A4TBlock()
        {
            Height = 210;
            Width = 297;
        }
    }
}
