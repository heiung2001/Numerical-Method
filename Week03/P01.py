import numpy as np


class P01:
    def __init__(self, _x: np.array, _y: np.array):
        self.x = _x
        self.y = _y

    def construct_divided_diff(self):
        n = len(self.x)
        a = np.zeros([n, n])
        a[:, 0] = self.y

        for j in range(1, n):
            for i in range(n-j):
                a[i, j] = (a[i+1][j-1]-a[i][j-1]) / (self.x[i + j] - self.x[i])
        return a

    def construct_poly_table(self):
        n = len(self.x)
        b = np.zeros([n, n+1])
        b[0][n-1] = 1

        for i in range(1, n):
            b[i][n-i-1] = 1
            for j in range(n-i, n):
                b[i][j] = b[i-1][j+1] - b[i-1][j]*self.x[i-1]
        return np.delete(b, -1, 1)

    def add_new_points(self, a: np.array, x: np.array, y: np.array):
        n = len(self.x)
        m = len(x)

        self.x = np.append(self.x, x)
        print(self.x)
        _a = np.zeros([n+m, n+m])
        _a[:n, :n] = a
        _a[n:, 0] = y

        for j in range(1, n+m):
            for i in range(n, n+m-j):
                _a[i, j] = (_a[i + 1][j - 1] - _a[i][j - 1]) / (self.x[i + j] - self.x[i])
        return _a


if __name__ == "__main__":
    X = np.array([2, 2.2, 2.4, 2.6, 2.8, 2.5, 2.1])
    Y = np.array([1.2837, 1.9034, 1.8885, 1.0938, 0.1662, 1.8179, 1.6924])
    X_new = np.array([2.3, 2.7])
    Y_new = np.array([1.8975, 0.8885])

    RUN = P01(X, Y)
    A = RUN.construct_divided_diff()
    B = RUN.construct_poly_table()

    # a1 = A[0, :]    # nội suy tiến
    # a2 = np.array([A[len(X)-i-1][i] for i in range(len(X))])    # nội suy lùi

    A_new = RUN.add_new_points(A, X_new, Y_new)

    X_test = np.array([2, 2.2, 2.4, 2.6, 2.8, 2.5, 2.1, 2.3, 2.7])
    Y_test = np.array([1.2837, 1.9034, 1.8885, 1.0938, 0.1662, 1.8179, 1.6924, 1.8975, 0.8885])
    p_test = P01(X_test, Y_test)
    A_test = p_test.construct_divided_diff()
    print(A_test - A_new)
