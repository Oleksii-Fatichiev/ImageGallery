using System;

namespace ImageGallery.Contracts.Exceptions
{
    public sealed class EntityNotFoundException
         : Exception
    {
        private const string DEFAULT_MESSAGE = "Entity not found exception";

        public Type EntityType { get; set; }

        public EntityNotFoundException(string message)
            : base(message) => EntityType = null;

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) => EntityType = null;

        public EntityNotFoundException()
            : base(DEFAULT_MESSAGE) => EntityType = null;

        public EntityNotFoundException(Type entityType)
            : base($"Entity of type {entityType?.Name} not found exception") => EntityType = entityType;
    }
}
