class Attempt {
	int x;
public:
	Attempt(int x);
	int adding(int y);
};

extern "C" _declspec(dllexport) void* Create(int x) {
	return (void*) new Attempt(x);
}

extern "C" _declspec(dllexport) int AttemptAdd(Attempt* a, int y) {
	return a->adding(y);
}