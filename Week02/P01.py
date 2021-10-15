import numpy as np


x = np.loadtxt(fname="inp_P01.txt", delimiter=' ')
n = len(x)

c = np.zeros(n+2)
c[n] = 1


def factorize():
    for i in range(n-1, -1, -1):
        c[i] = 1
        for j in range(i+1, n+1):
            c[j] = c[j+1] - c[j]*x[i]


if __name__ == "__main__":
    factorize()
    print(c)
