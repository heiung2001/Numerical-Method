import numpy as np


def factorize(a: np.array):
    n = len(a)
    c = np.zeros(n + 2)
    c[n] = 1

    for i in range(n-1, -1, -1):
        c[i] = 1
        for j in range(i+1, n+1):
            c[j] = c[j+1] - c[j]*a[i]
    return c[:n+1]


class P04:
    points = np.loadtxt("inp_P02.txt", delimiter=' ')
    omega = factorize(points[:, 0])

    def find_d(self):
        a = np.tile(self.points[:, 0], (len(self.points), 1))
        for i in range(len(self.points)):
            a[i] = a[i] - self.points[i, 0]
        np.fill_diagonal(a, 1)
        a = np.prod(a, axis=0)
        return a

    def horner(self):
        n = len(self.points)
        res = np.zeros([n, n])
        for k, elm in enumerate(self.points[:, 0]):
            res[k][0] = self.omega[0]
            for idx in range(1, n):
                res[k][idx] = res[k][idx-1]*elm + self.omega[idx]
        return res

    def perform_method_2(self):
        d = self.find_d()
        for k in range(len(self.points)):
            d[k] = self.points[k, 1]/d[k]
        a = self.horner()
        res = d.dot(a)
        print(res)

    def perform_method_1(self):
        d = self.find_d()
        res = np.zeros(len(self.points))
        for k in range(len(self.points)):
            c = self.points[k, 1]/d[k]
            temp = np.delete(self.points[:, 0], k)
            a = factorize(temp)
            res += c*a
        print(res)


if __name__ == "__main__":
    RUN = P04()
    RUN.perform_method_2()
