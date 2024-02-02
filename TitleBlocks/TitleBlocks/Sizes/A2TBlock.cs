using TitleBlocks.Model;

namespace TitleBlocks.Sizes
{
    public class A2TBlock : TBlock
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public A2TBlock()
        {
            Height = 420;
            Width = 594;
        }
    }
}
