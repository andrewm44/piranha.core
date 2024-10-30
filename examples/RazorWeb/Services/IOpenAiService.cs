namespace NoisyWeb.Services
{
    public interface IOpenAiService
    {
        public Task<string> EnhanceText(string inText, int numWords);
    }
}
