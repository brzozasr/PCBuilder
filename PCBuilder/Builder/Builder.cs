using PCBuilder.PCs;

namespace PCBuilder.Builder
{
    public class Builder : IBuilder
    {
        private Pc _pc = new();

        public Builder()
        {
            Reset();
        }
        
        private void Reset()
        {
            _pc = new Pc();
        }

        public void BuildPc()
        {
            _pc.BuildPcParts();
        }

        public Pc GetPc()
        {
            Pc result = _pc;
            Reset();

            return result;
        }
    }
}