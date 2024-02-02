using TitleBlocks.Model;

namespace TitleBlocks.Sizes
{
    public class A1TBlock : TBlock
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public A1TBlock()
        {
            Height = 594;
            Width = 841;
        }
    }
}
