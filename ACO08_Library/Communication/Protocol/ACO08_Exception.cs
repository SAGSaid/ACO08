using System;
using System.Linq;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    public class ACO08_Exception : Exception
    {
        public ErrorId ErrorId { get; }

        public ACO08_Exception(ErrorId id)
        {
            ErrorId = id;
        }

        internal static void ThrowOnResponseError(CommandResponse response)
        {
            if (response.IsError)
            {
                throw new ACO08_Exception((ErrorId)response.GetBody().Last());
            }
        }
    }
}
