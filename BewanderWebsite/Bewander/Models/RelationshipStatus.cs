using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    public enum RelationshipStatus
    {
        Pending = 0,
        Friends = 1,
        Denied = 2,
        Blocked = 3,
        NoRelationship = 4
    }
}