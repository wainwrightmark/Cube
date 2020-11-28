namespace CombinationPuzzle
{
    public static class Miscellaneous {

        // ######################################## Miscellaneous functions #####################################################
        // "Rotate array arr right between l and r. r is included.
        public static void rotate_right<T>(this T[] arr, int l, int r) { //TODO change to use span or memory
            var temp = arr[r];
            for (var i = r; i > l; i--)
                arr[i] = arr[i - 1];
            arr[l] = temp;
        }

        // "Rotate array arr left between l and r. r is included.
        public static void rotate_left<T>(this T[] arr, int l, int r) { //TODO change to use span or memory
            var temp = arr[l];

            for (var i = l; i < r; i++) arr[i] = arr[i + 1];

            arr[r] = temp;
        }

        // Binomial coefficient [n choose k].
        public static uint BinomialChoose(uint n, uint k) { //TODO see if there is a library

            if (n < k) {
                return 0;
            }
            if (k > n / 2) {
                k = n - k;
            }
            uint s = 1;
            var i = n;
            uint j = 1;
            while (i != n - k) {
                s *= i;
                s /= j;
                i -= 1;
                j += 1;
            }
            return s;
        }

        public static int BinomialChoose(int n, int k)
        { //TODO see if there is a library

            if (n < k)
            {
                return 0;
            }
            if (k > n / 2)
            {
                k = n - k;
            }
            int s = 1;
            var i = n;
            int j = 1;
            while (i != n - k)
            {
                s *= i;
                s /= j;
                i -= 1;
                j += 1;
            }
            return s;
        }
    }
}
