public class UnsafeFunction {
    public unsafe static void memcpyimpl(byte* src, byte* dest, int len) {
        if (len >= 16) {
            do {
                *(int*)dest = *(int*)src;
                *(int*)(dest + 4) = *(int*)(src + 4);
                *(int*)(dest + 8) = *(int*)(src + 8);
                *(int*)(dest + 12) = *(int*)(src + 12);
                dest += 16;
                src += 16;
            }
            while ((len -= 16) >= 16);
        }
        if (len > 0) {
            if ((len & 8) != 0) {
                *(int*)dest = *(int*)src;
                *(int*)(dest + 4) = *(int*)(src + 4);
                dest += 8;
                src += 8;
            }
            if ((len & 4) != 0) {
                *(int*)dest = *(int*)src;
                dest += 4;
                src += 4;
            }
            if ((len & 2) != 0) {
                *(short*)dest = *(short*)src;
                dest += 2;
                src += 2;
            }
            if ((len & 1) != 0) {
                *(dest++) = *(src++);
            }
        }
    }
}
