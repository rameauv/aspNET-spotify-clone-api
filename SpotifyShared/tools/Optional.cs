namespace Spotify.Shared.tools;

public struct Optional<T>
{
    public bool HasValue { get; private set; }
    private T _value;

    public T Value
    {
        get
        {
            if (HasValue)
                return _value;
            else
                throw new InvalidOperationException();
        }
    }

    public Optional(T value)
    {
        this._value = value;
        HasValue = true;
    }

    public static explicit operator T(Optional<T> optional)
    {
        return optional.Value;
    }

    public static implicit operator Optional<T>(T value)
    {
        return new Optional<T>(value);
    }

    public override bool Equals(object obj)
    {
        if (obj is Optional<T>)
            return this.Equals((Optional<T>)obj);
        else
            return false;
    }

    public bool Equals(Optional<T> other)
    {
        if (HasValue && other.HasValue)
            return object.Equals(_value, other._value);
        else
            return HasValue == other.HasValue;
    }
}