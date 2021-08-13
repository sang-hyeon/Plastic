namespace Plastic
{
    using System;
    using System.Collections.Generic;

    public delegate IEnumerable<Pipe> BuildPipeline(IServiceProvider provider);
}
