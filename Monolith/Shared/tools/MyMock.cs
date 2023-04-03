using Moq;

namespace Spotify.Shared.tools;

public class MyMock<T> : Mock<T> where T : class
{
    public MyMock() : base()
    {
        base.DefaultValueProvider = new CustomValueProvider();
    }
}