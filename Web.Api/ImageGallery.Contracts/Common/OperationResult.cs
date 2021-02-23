using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Contracts.Common
{
    public sealed class OperationResult<T>
    {
        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }
        public T Data { get; }

        public string ErrorMessage => string.Join(Environment.NewLine, Errors ?? Enumerable.Empty<string>());

        public static OperationResult<T> Success(T result) =>
            new OperationResult<T>(result, Enumerable.Empty<string>(), true);

        public static OperationResult<T> Failed(params string[] errors) =>
            Failed((IEnumerable<string>)errors);

        public static OperationResult<T> Failed(IEnumerable<string> errors) =>
            new OperationResult<T>(default, errors ?? Enumerable.Empty<string>(), false);

        private OperationResult(T result, IEnumerable<string> errors, bool success)
        {
            Data = result;
            Errors = errors;
            Succeeded = success;
        }
    }

    public sealed class OperationResult
    {
        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; set; }
        public string ErrorMessage =>
            string.Join(Environment.NewLine, Errors ?? Enumerable.Empty<string>());
        public static OperationResult Success { get; private set; }

        private OperationResult()
        {
        }

        static OperationResult() =>
            Success = new OperationResult
            {
                Succeeded = true,
                Errors = Enumerable.Empty<string>()
            };

        public static OperationResult Failed(IEnumerable<string> errors) =>
            new OperationResult
            {
                Succeeded = false,
                Errors = errors ?? Enumerable.Empty<string>()
            };

        public static OperationResult Failed(params string[] errors) => Failed((IEnumerable<string>)errors);
    }
}
