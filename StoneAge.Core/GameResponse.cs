namespace StoneAge.Core
{
    public class GameResponse<T> : GameResponse
    {
        public readonly T Value;

        public static GameResponse<T> Pass(T responseObject)
        {
            return new GameResponse<T>(responseObject, true);
        }

        public static GameResponse<T> Fail(T responseObject)
        {
            return new GameResponse<T>(responseObject, false);
        }

        public new static GameResponse<T> Fail()
        {
            return new GameResponse<T>(default(T), false);
        }

        private GameResponse(T responseObject, bool success)
            : base(success)
        {
            Value = responseObject;
        }
    }

    public class GameResponse
    {
        public readonly bool Successful;

        public static GameResponse Pass()
        {
            return new GameResponse(true);
        }

        public static GameResponse Fail()
        {
            return new GameResponse(false);
        }

        protected GameResponse(bool success)
        {
            Successful = success;
        }
    }
}
