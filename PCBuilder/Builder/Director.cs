namespace PCBuilder.Builder
{
    public class Director
    {
        public IBuilder Builder { get; set; }

        public void BuildComputer()
        {
            Builder.BuildPc();
        }
    }
}