using System.Collections.Generic;

namespace LH.Stats {
    public interface IStatModifier {
        IEnumerable<float>  GetModifier(Stat stat);
        IEnumerable<float>  GetMultiplier(Stat stat);
    }
}
