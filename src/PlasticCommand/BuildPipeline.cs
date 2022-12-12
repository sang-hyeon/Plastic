using System;
using System.Collections.Generic;

namespace PlasticCommand;

public delegate IEnumerable<IPipe> BuildPipeline(IServiceProvider provider);
