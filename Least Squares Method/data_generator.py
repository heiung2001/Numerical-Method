from math import exp, log, sin, cos
import random as rd

n = 15
x = [0, 0.2, 0.3, 0.5, 0.6, 0.7, 0.9, 1.1, 1.2, 1.4, 1.5, 1.6, 1.8, 1.9, 2]
y = []

# POLYNOMIAL
for i in range(len(x)):
    data = 7.5 + 2*x[i]**3 - 6*x[i] + rd.uniform(2.5, 3.5)
    y.append(data)

with open("data1.txt", 'w') as f:
    for i in range(len(x)):
        f.write(str(x[i]) + '\t' + str(y[i]) + '\n')


# TRIGONOMETRY
y.clear()
for i in range(len(x)):
    data = 8.2 + 4.4*sin(x[i]) - 11*sin(2*x[i]) + rd.uniform(2.5, 3.5)
    y.append(data)

with open("data2.txt", "w") as f:
    for i in range(len(x)):
        f.write(str(x[i]) + '\t' + str(y[i]) + '\n')

# EXPONENTIAL
y.clear()
for i in range(len(x)):
    data = 3.7 * exp(-9*x[i]**3 + 6*x[i]**2 + x[i]**5) + rd.uniform(2.5, 3.5)
    y.append(data)

with open("data3.txt", "w") as f:
    for i in range(len(x)):
        f.write(str(x[i]) + '\t' + str(y[i]) + '\n')


# LOGARITHM
y.clear()
for i in range(len(x)):
    data = log(7.5 + 2*x[i]**3 - 6*x[i]) + rd.uniform(2.8, 3)
    y.append(data)

with open("data4.txt", "w") as f:
    for i in range(len(x)):
        f.write(str(x[i]) + '\t' + str(y[i]) + '\n')
