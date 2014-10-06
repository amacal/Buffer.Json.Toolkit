namespace Buffer.Json
{
    public interface IHandler
    {
        void HandleBegin();

        void HandleArrayBegin();

        void HandleArraySeparator();

        void HandleArrayEnd();

        void HandleObjectBegin();

        void HandleObjectProperty();

        void HandleObjectPropertySeparator();

        void HandleObjectValue();

        void HandleObjectSeparator();

        void HandleObjectEnd(int count);

        void HandleStringBegin();

        void HandleStringEscapeCharacter();

        void HandleStringEscapeValue(byte value);

        void HandleStringEscapeUnicode(byte[] value);

        void HandleStringCharacter(byte value);

        void HandleStringEnd();

        void HandleNumberSign();

        void HandleNumberValue(byte value);

        void HandleNumberSeparator();

        void HandleNumberDecimal(byte value);

        void HandleNumberExponent(byte value);

        void HandleNumberExponentSign(byte value);

        void HandleNumberExponentValue(byte value);

        void HandleNullValue();

        void HandleTrueValue();

        void HandleFalseValue();

        void HandleWhiteCharacter(byte value);

        void HandleEnd();
    }
}
