namespace RhoMicro.TplEx;
using System;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Interface,
    AllowMultiple = false,
    Inherited = false)]
#if RHOMICRO_TPLEX_GENERATORS
[CodeAnalysis.IncludeFile]
[CodeAnalysis.GenerateFactory]
#endif
#if GENERATOR
[CodeAnalysis.NonEquatable]
#endif
internal sealed partial class ExtendTplAttribute : Attribute { }
