using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoshinoLabs.VRC.Sardinject {
    public enum Lifetime {
        Transient,
        Cached,
        Scoped,
    }
}