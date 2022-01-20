﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shtormtech.configuration.common.Enums
{
    public enum MergeStatus
    {
        //
        // Summary:
        //     Merge was up-to-date.
        UpToDate,
        //
        // Summary:
        //     Fast-forward merge.
        FastForward,
        //
        // Summary:
        //     Non-fast-forward merge.
        NonFastForward,
        //
        // Summary:
        //     Merge resulted in conflicts.
        Conflicts
    }
}
