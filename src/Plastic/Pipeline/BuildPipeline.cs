namespace Plastic
{
    using System;
    using System.Collections.Generic;

    public delegate IEnumerable<IPipe> BuildPipeline(IServiceProvider provider);
}
