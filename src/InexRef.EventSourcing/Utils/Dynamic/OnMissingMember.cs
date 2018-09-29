namespace InexRef.EventSourcing.Utils.Dynamic
{
    public class OnMissingMember
    {
        private OnMissingMember(bool throwException, object returnValue)
        {
            Exception = throwException;
            ReturnValue = returnValue;
        }

        public static OnMissingMember ThrowException() => new OnMissingMember(true, null);

        public static OnMissingMember Ignore(object returnValue = null) => new OnMissingMember(false, returnValue);

        public bool Exception { get; }

        public object ReturnValue { get; }
    }
}