using Application.Common.Extensions.Enumerator;

namespace Application.Common.Helpers.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(BusinessExceptionType tipoException) : base(tipoException.GetDescription())
        {
        }

        public BusinessException(BusinessExceptionType tipoException, params object[] data) : base(string.Format(tipoException.GetDescription(), data))
        {
        }
    }
}
