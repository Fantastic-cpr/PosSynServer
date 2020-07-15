using System.Collections.Generic;

namespace ConnectBridge.Util {
    public class DictUtil {
        public static V GetValue<K, V>(Dictionary<K, V> dict, K key) {
            return dict.TryGetValue(key, out var value)?value:default(V);
        }
    }
}