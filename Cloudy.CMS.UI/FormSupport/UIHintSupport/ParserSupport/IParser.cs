namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public interface IParser
    {
        void ExpectEnd();
        void Expect(params char[] tokens);
        void ExpectOrEnd(params char[] tokens);
        string ReadUntil(params char[] tokens);
        void ContinueUntil(char[] tokens);
        void SkipWhitespace();
        void ContinueWhile(params char[] tokens);
        void ReverseWhile(params char[] tokens);

        void Skip();
        bool IsStart();
        bool IsEnd();
        bool Is(params char[] tokens);
        bool Was(params char[] tokens);
    }
}