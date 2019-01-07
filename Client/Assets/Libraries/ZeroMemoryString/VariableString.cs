public class VariableString {
    private string _data;
    private int maxCount;

    public VariableString(int maxCount = 1024) {
        this.maxCount = maxCount;
        _data = new string('\0', this.maxCount);
        Clear();
    }

    public string GetString() {
        return _data;
    }

    public string ConCat(string a, string b, bool clear = true) {
        if (clear) {
            Clear();
        }
        Push(a);
        Push(b);
        return _data;
    }

    public unsafe void Push(string newStr) {
        if (string.IsNullOrEmpty(newStr)) {
            return;
        }

        int newLen = _data.Length + newStr.Length;
        if (newLen >= maxCount) {
            return;
        }

        fixed (char* src = newStr) {
            fixed (char* dst = _data) {
                UnsafeFunction.memcpyimpl((byte*)src, (byte*)(dst + _data.Length), newStr.Length * 2); //system.string的存储每个元素两个字节
                int* iDst = (int*)dst;
                iDst = iDst - 1;    //字符串的长度在第一个元素的前面4个字节
                *iDst = newLen;
            }
        }
    }

    public unsafe void Clear() {
        fixed (char* p = _data) {
            int* pSize = (int*)p;
            pSize = pSize - 1;
            *pSize = 0;
        }
    }
}
