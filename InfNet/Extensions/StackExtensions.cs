using System.Collections.Generic;

namespace InfNet.Extensions {
    public static class StackExtensions {
        public static T PushAndReturn<T>(this Stack<T> stack, T value) {
            stack.Push(value);
            return value;
        }
    }
}
