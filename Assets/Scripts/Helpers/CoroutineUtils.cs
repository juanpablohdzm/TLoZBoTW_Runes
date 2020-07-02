using System.Collections;

namespace Utilities {
    public static class CoroutineUtils { 
        public static IEnumerator UntilEither(IEnumerator c1, IEnumerator c2)
        {
            while (true) {
                var stopEither = !c1.MoveNext() || !c2.MoveNext();
                if (stopEither)
                    yield break;
                else
                    yield return null;
            }
        }
    }
}