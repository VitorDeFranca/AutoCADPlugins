using TitleBlocks.Model;

namespace TitleBlocks.Sizes
{
    public class A0TBlock : TBlock
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public A0TBlock()
        {
            Height = 841;
            Width = 1189;
        }
    }
}
