﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Commentaries.Models
{
    public sealed record AddCommentaryRequest(Guid TaskId, string Commentary, Guid UserId);
}
