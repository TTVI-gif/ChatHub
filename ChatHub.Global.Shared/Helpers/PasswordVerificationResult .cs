using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Global.Shared.Helpers
{
    public enum PasswordVerificationResult : byte
    {
        Failed,
        Success,
        SuccessRehashNeeded
    }
}
